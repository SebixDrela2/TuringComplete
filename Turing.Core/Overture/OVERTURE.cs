using Turing.Core.Components.Memory;
using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Overture;

/// <summary>
/// Overture Register File (Phase 1)
/// Instruction format: bits 0‑2 = source, bits 3‑5 = destination.
/// Source/destination mapping:
///   0‑5: REG0‑REG5
///   6:   INPUT (source) / OUTPUT (destination)
///   7:   UNUSED
/// </summary>
public class OVERTURE
{
    private readonly REGISTER<Byte>[] _regs;
    private Byte _output;

    public Byte Output => _output;

    public OVERTURE()
    {
        _regs = new REGISTER<Byte>[6];
        for (int i = 0; i < 6; i++)
            _regs[i] = new REGISTER<Byte>();
        _output = new Byte(0);
    }

    /// <summary>
    /// Evaluate the register file on each clock tick.
    /// </summary>
    /// <param name="instruction">The instruction byte</param>
    /// <param name="inputData">External input data</param>
    /// <param name="tick">Clock tick (write on rising edge)</param>
    public void EVal(Byte instruction, Byte inputData, Bit tick)
    {
        // Extract source select bits (bits 0,1,2)
        Bit src0 = instruction.GetBit(0);
        Bit src1 = instruction.GetBit(1);
        Bit src2 = instruction.GetBit(2);
        // Extract destination select bits (bits 3,4,5)
        Bit dst0 = instruction.GetBit(3);
        Bit dst1 = instruction.GetBit(4);
        Bit dst2 = instruction.GetBit(5);

        // Decode source and destination using BIT_DECODER_THREE (disable = 0)
        // IMPORTANT: BIT_DECODER_THREE expects MSB first: (MSB, mid, LSB, disable)
        Byte srcDecoded = new BIT_DECODER_THREE(src2, src1, src0, new Bit(false));
        Byte dstDecoded = new BIT_DECODER_THREE(dst2, dst1, dst0, new Bit(false));

        // Select the source value (combine registers, input, and zero)
        Byte srcValue = SelectSource(srcDecoded, inputData);

        // Write to registers (0‑5) if the corresponding destination bit is set
        for (int i = 0; i < 6; i++)
        {
            Bit load = new Bit((bool)dstDecoded.GetBit(i));
            _regs[i].EVal(load, srcValue, tick);
        }

        // Output: if destination is 6, set output to srcValue; otherwise 0
        Bit outEnable = new Bit((bool)dstDecoded.GetBit(6));
        _output = outEnable.Value ? srcValue : new Byte(0);
    }

    /// <summary>
    /// Select the source value based on the decoded source bits.
    /// </summary>
    private Byte SelectSource(Byte srcDecoded, Byte inputData)
    {
        Byte result = new Byte(0);

        // Registers 0‑5
        for (int i = 0; i < 6; i++)
        {
            Bit enable = new Bit((bool)srcDecoded.GetBit(i));
            Byte masked = new AND<Byte>(_regs[i].State, new Byte(enable.Value ? 0xFF : 0x00));
            result = new OR<Byte>(result, masked);
        }

        // Input (source = 6)
        Bit enableInput = new Bit((bool)srcDecoded.GetBit(6));
        Byte maskedInput = new AND<Byte>(inputData, new Byte(enableInput.Value ? 0xFF : 0x00));
        result = new OR<Byte>(result, maskedInput);

        // Source 7 is unused → contributes nothing
        return result;
    }

    public static implicit operator Byte(OVERTURE overture)
    {
        return overture._output;
    }
}