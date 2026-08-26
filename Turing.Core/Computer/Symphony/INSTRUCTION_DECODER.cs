using Turing.Core.ComplexComponents;
using Turing.Core.Electricity;

namespace Turing.Core.Computer.Symphony;

/// <summary>
/// <br>Symphony instruction decoder, takes INT as an input and outputs Bytes for Mode,OpOpcode,Destination,A,B,IsImmiediate,ImmiediateValue.</br>
/// </summary>
/// <param name="instruction"></param>
internal class INSTRUCTION_DECODER(Int instruction) : TurComponent<(Byte Mode, Byte OpCode, Byte Destination, Byte A, Byte B, Bit IsImm, Short ImmVal)>
{
    protected override (Byte Mode, Byte OpCode, Byte Destination, Byte A, Byte B, Bit IsImm, Short ImmVal) ImplicitOperator()
    {
        Byte mode = ExecTo<Byte>(2, new SINDEXER<Int>(instruction, 29));
        Bit isImm = ExecTo<Bit>(1, new SINDEXER<Int>(instruction, 28));
        Byte opCode = ExecTo<Byte>(4, new SINDEXER<Int>(instruction, 24));
        Byte dest = ExecTo<Byte>(4, new SINDEXER<Int>(instruction, 20));
        Byte a = ExecTo<Byte>(4, new SINDEXER<Int>(instruction, 16));
        Byte b = ExecTo<Byte>(4, new SINDEXER<Int>(instruction, 8));
        Short immVal = ExecTo<Short>(new SINDEXER<Int>(instruction, 0));

        return (mode, opCode, dest, a, b, isImm, immVal);
    }
}
