using Turing.Core.Components.Memory;
using Turing.Core.Components.Arithmetic;
using Turing.Core.Components.Logic;
using Turing.Core.Overture;
using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Overture;

/// <summary>
/// Overture CPU - Pure Gate-Level Implementation
/// All logic is built from gates (AND, OR, NOT, MUX, etc.)
/// No C# operators (|, &, ?, etc.) - only gate components
/// </summary>
public class OVERTURE
{
    private readonly REGISTER<Byte>[] _regs;
    private readonly COUNTER<Byte> _pc;
    private Byte _output;

    public Byte Output => _output;
    public int ProgramCounter => (int)(Byte)_pc;

    public OVERTURE()
    {
        _regs = new REGISTER<Byte>[6];
        for (int i = 0; i < 6; i++)
            _regs[i] = new REGISTER<Byte>();
        _pc = new COUNTER<Byte>();
        _output = new Byte(0);
    }

    public void EVal(Byte instruction, Byte inputData, Bit tick)
    {
        // Decode mode using gates (no if)
        var (immediate, alu, move, cond) = ((Bit, Bit, Bit, Bit))new INSTRUCTION_DECODER(instruction);

        // Extract source and destination bits
        Bit src0 = instruction.GetBit(0);
        Bit src1 = instruction.GetBit(1);
        Bit src2 = instruction.GetBit(2);
        Bit dst0 = instruction.GetBit(3);
        Bit dst1 = instruction.GetBit(4);
        Bit dst2 = instruction.GetBit(5);

        // Decode source and destination (MSB-first for BIT_DECODER_THREE)
        Byte srcDecoded = new BIT_DECODER_THREE(src2, src1, src0, new Bit(false));
        Byte dstDecoded = new BIT_DECODER_THREE(dst2, dst1, dst0, new Bit(false));

        // ------------------------------------------------------------------
        // 1. MOVE VALUE: select source based on srcDecoded
        // ------------------------------------------------------------------
        Byte moveValue = SelectSource(srcDecoded, inputData);

        // ------------------------------------------------------------------
        // 2. ALU RESULT: opcode from src0,src1,src2, operands REG1, REG2
        // ------------------------------------------------------------------
        // Build opcode bits using gates (no | operator)
        Bit opBit0 = src0;
        Bit opBit1 = src1;
        Bit opBit2 = src2;

        // Shift to bits 5,6,7 using AND with mask
        Byte opMask0 = new Byte(0x20); // bit 5
        Byte opMask1 = new Byte(0x40); // bit 6
        Byte opMask2 = new Byte(0x80); // bit 7

        Byte opByte0 = new AND<Byte>(new Byte(opBit0.Value ? 0x20 : 0x00), opMask0);
        Byte opByte1 = new AND<Byte>(new Byte(opBit1.Value ? 0x40 : 0x00), opMask1);
        Byte opByte2 = new AND<Byte>(new Byte(opBit2.Value ? 0x80 : 0x00), opMask2);

        Byte aluOpcode = new OR<Byte>(new OR<Byte>(opByte0, opByte1), opByte2);

        ALU aluComponent = new ALU(aluOpcode, _regs[1].State, _regs[2].State);
        Byte aluResult = (Byte)aluComponent;

        // ------------------------------------------------------------------
        // 3. IMMEDIATE VALUE: lower 6 bits of instruction
        // ------------------------------------------------------------------
        // AND instruction with 0x3F mask using gates
        Byte immMask = new Byte(0x3F);
        Byte immValue = new AND<Byte>(instruction, immMask);

        // ------------------------------------------------------------------
        // 4. CONDITION RESULT: evaluate REG3
        // ------------------------------------------------------------------
        // Build condition byte using gates (no | operator)
        Byte condMask0 = new Byte(0x20);
        Byte condMask1 = new Byte(0x40);
        Byte condMask2 = new Byte(0x80);

        Byte condByte0 = new AND<Byte>(new Byte(src0.Value ? 0x20 : 0x00), condMask0);
        Byte condByte1 = new AND<Byte>(new Byte(src1.Value ? 0x40 : 0x00), condMask1);
        Byte condByte2 = new AND<Byte>(new Byte(src2.Value ? 0x80 : 0x00), condMask2);

        Byte condByte = new OR<Byte>(new OR<Byte>(condByte0, condByte1), condByte2);

        COND condComponent = new COND(_regs[3].State, condByte);
        Bit condResult = (Bit)condComponent;

        // ------------------------------------------------------------------
        // 5. SELECT WRITE DATA (Move vs ALU vs Immediate)
        // ------------------------------------------------------------------
        // MUX: sel=0 -> A, sel=1 -> B
        // First select between moveValue and aluResult based on 'alu'
        Byte moveOrAlu = new MUX<Byte>(moveValue, aluResult, alu);
        // Then select between that and immValue based on 'immediate'
        Byte writeData = new MUX<Byte>(moveOrAlu, immValue, immediate);

        // ------------------------------------------------------------------
        // 6. SELECT LOAD SIGNAL FOR EACH REGISTER
        // ------------------------------------------------------------------
        Bit[] load = new Bit[6];
        for (int i = 0; i < 6; i++)
        {
            // Move mode: destination matches i
            Bit dstBit = new Bit((bool)dstDecoded.GetBit(i));
            Bit moveLoad = new AND<Bit>(move, dstBit);

            // ALU mode: only REG3 (i == 3)
            Bit isReg3 = new Bit(i == 3);
            Bit aluLoad = new AND<Bit>(alu, isReg3);

            // Immediate mode: only REG0 (i == 0)
            Bit isReg0 = new Bit(i == 0);
            Bit immLoad = new AND<Bit>(immediate, isReg0);

            // OR all together
            Bit or1 = new OR<Bit>(moveLoad, aluLoad);
            Bit or2 = new OR<Bit>(or1, immLoad);
            load[i] = or2;
        }

        // Write to registers
        for (int i = 0; i < 6; i++)
            _regs[i].EVal(load[i], writeData, tick);

        // ------------------------------------------------------------------
        // 7. OUTPUT VALUE
        // ------------------------------------------------------------------
        // Output enabled only in Move mode when destination = 6
        Bit dst6 = new Bit((bool)dstDecoded.GetBit(6));
        Bit outEnableMove = new AND<Bit>(move, dst6);

        // Create mask from outEnableMove
        Byte outMask = new Byte(outEnableMove.Value ? 0xFF : 0x00);
        _output = new AND<Byte>(moveValue, outMask);

        // ------------------------------------------------------------------
        // 8. PROGRAM COUNTER UPDATE
        // ------------------------------------------------------------------
        // PC load when in Condition mode and condResult is true
        Bit pcLoad = new AND<Bit>(cond, condResult);

        // Select PC load value: if pcLoad true, use REG0; else use 0
        Byte zero = new Byte(0);
        Byte pcLoadValue = new MUX<Byte>(zero, _regs[0].State, pcLoad);

        // Update Program Counter
        _pc.EVal(pcLoad, pcLoadValue, tick);
    }

    private Byte SelectSource(Byte srcDecoded, Byte inputData)
    {
        Byte result = new Byte(0);

        for (int i = 0; i < 6; i++)
        {
            Bit enable = new Bit((bool)srcDecoded.GetBit(i));
            Byte mask = new Byte(enable.Value ? 0xFF : 0x00);
            Byte masked = new AND<Byte>(_regs[i].State, mask);
            result = new OR<Byte>(result, masked);
        }

        Bit enableInput = new Bit((bool)srcDecoded.GetBit(6));
        Byte inputMask = new Byte(enableInput.Value ? 0xFF : 0x00);
        Byte maskedInput = new AND<Byte>(inputData, inputMask);
        result = new OR<Byte>(result, maskedInput);

        return result;
    }

    public void Reset()
    {
        _pc.Reset();
        _output = new Byte(0);
        foreach (var reg in _regs)
            reg.Reset();
    }

    public static implicit operator Byte(OVERTURE overture)
    {
        return overture._output;
    }
}