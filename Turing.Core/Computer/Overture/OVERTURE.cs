using Turing.Core.Components.Memory;
using Turing.Core.Components.Logic;
using Turing.Core.Gates;
using Turing.Core.Gates.Primitives;
using Turing.Core.Computers;

namespace Turing.Core.Overture;

public class OVERTURE : Processor<Byte>
{
    private readonly REGISTER<Byte>[] _regs;
    private readonly COUNTER<Byte> _pc;
    private Byte _output;
    public Byte Output => _output;
    public int ProgramCounter => (int)_pc.State;

    public OVERTURE()
    {
        _regs = new REGISTER<Byte>[6];

        for (int i = 0; i < 6; i++)
        {
            _regs[i] = new REGISTER<Byte>(Clock);
        }

        _pc = new COUNTER<Byte>(Clock);
        _output = new Byte(0);
    }

    protected override void Step(Byte instruction, Byte inputData)
    {
        (Bit imm, Bit alu, Bit move, Bit cond) = ((Bit, Bit, Bit, Bit))new INSTRUCTION_DECODER(instruction);

        Bit y0 = instruction.GetBit(0);
        Bit y1 = instruction.GetBit(1);
        Bit y2 = instruction.GetBit(2);

        Bit y3 = instruction.GetBit(3);
        Bit y4 = instruction.GetBit(4);
        Bit y5 = instruction.GetBit(5);

        Bit y6 = instruction.GetBit(6);
        Bit y7 = instruction.GetBit(7);

        Byte srcDecoder = new BIT_DECODER_THREE(y2, y1, y0, new NOT<Bit>(move));
        Byte dstDecoder = new BIT_DECODER_THREE(y5, y4, y3, new NOT<Bit>(move));

        Byte inputFlow = new MUX<Byte>(inputData, instruction, imm);
        Bit src_in = srcDecoder.GetBit(6);

        inputFlow = new SW<Byte>(new OR<Bit>(src_in, imm), inputFlow);

        SetInputs(instruction, srcDecoder, dstDecoder, inputFlow, imm, alu);
        SetOutput(srcDecoder, dstDecoder, inputFlow);
        SetCounter(instruction, cond);
    }

    private void SetInputs(Byte instruction, Byte inByte, Byte outByte, Byte inputFlow, Bit imm, Bit alu)
    {
        var r0_in = outByte.GetBit(0);
        var r1_in = outByte.GetBit(1);
        var r2_in = outByte.GetBit(2);
        var r3_in = outByte.GetBit(3);
        var r4_in = outByte.GetBit(4);
        var r5_in = outByte.GetBit(5);

        _regs[0].EVal(new OR<Bit>(r0_in, imm), inputFlow);
        _regs[1].EVal(r1_in, inputFlow);
        _regs[2].EVal(r2_in, inputFlow);

        Byte aluResult = new ALU(instruction, _regs[1], _regs[2]);
        var reg3Input = new OR<Byte>(inputFlow, new SW<Byte>(alu, aluResult));

        _regs[3].EVal(new OR<Bit>(r3_in, alu), reg3Input);
        _regs[4].EVal(r4_in, inputFlow);
        _regs[5].EVal(r5_in, inputFlow);

        for (int i = 0; i < _regs.Length; i++)
        {
            var r_out = inByte.GetBit(i);

            for (int j = 0; j < _regs.Length; j++)
            {
                var r_in = outByte.GetBit(j);
                var evalReg = new AND<Bit>(r_out, r_in);

                _regs[j].EVal(evalReg, _regs[i]);
            }
        }
    }

    private void SetOutput(Byte instruction, Byte outByte, Byte inputFlow)
    {
        var r0_out = instruction.GetBit(0);
        var r1_out = instruction.GetBit(1);
        var r2_out = instruction.GetBit(2);
        var r3_out = instruction.GetBit(3);
        var r4_out = instruction.GetBit(4);
        var r5_out = instruction.GetBit(5);
        var in_out = instruction.GetBit(6);

        var r0result = new SW<Byte>(r0_out, _regs[0]);
        var r1result = new SW<Byte>(r1_out, _regs[1]);
        var r2result = new SW<Byte>(r2_out, _regs[2]);
        var r3result = new SW<Byte>(r3_out, _regs[3]);
        var r4result = new SW<Byte>(r4_out, _regs[4]);
        var r5result = new SW<Byte>(r5_out, _regs[5]);
        var in_result = new SW<Byte>(in_out, inputFlow);

        var pair1 = new OR<Byte>(r0result, r1result);
        var pair2 = new OR<Byte>(r2result, r3result);
        var pair3 = new OR<Byte>(r4result, r5result);

        var result1 = new OR<Byte>(pair1, pair2);
        var result2 = new OR<Byte>(pair3, in_result);

        var outResult = new OR<Byte>(result1, result2);
        var outputBit = outByte.GetBit(6);

        _output = new SW<Byte>(outputBit, outResult);
    }

    private void SetCounter(Byte instruction, Bit cond)
    {
        Byte r3_out = _regs[3];
        Byte r0_out = _regs[0];

        Bit condResult = new COND(r3_out, instruction);

        _pc.EVal(new AND<Bit>(cond, condResult), r0_out);
    }

    public void Reset()
    {
        _pc.Reset();
        _output = new Byte(0);

        foreach (var reg in _regs)
        {
            reg.Reset();
        }
    }

    public Byte GetOutput() => _output;
}