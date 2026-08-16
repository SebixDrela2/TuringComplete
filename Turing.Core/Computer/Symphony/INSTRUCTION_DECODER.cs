using Turing.Core.ComplexComponents;
using Turing.Core.Electricity;

namespace Turing.Core.Computer.Symphony;

internal class INSTRUCTION_DECODER(Int instruction) : TurComponent<(Byte Mode, Byte OpCode, Byte Destination, Byte A, Byte B, Bit IsImm, Short ImmVal)>
{
    protected override (Byte Mode, Byte OpCode, Byte Destination, Byte A, Byte B, Bit IsImm, Short ImmVal) ImplicitOperator()
    {
        Byte mode = ExecTo<Byte>(new LSR<Int>(instruction, 29));
        Byte opCode = ExecTo<Byte>(new LSR<Int>(instruction, 24));
        Byte dest = ExecTo<Byte>(new LSR<Int>(instruction, 20));
        Byte a = ExecTo<Byte>(new LSR<Int>(instruction, 16));
        Byte b = ExecTo<Byte>(new LSR<Int>(instruction, 8));
        Bit isImm = ExecTo<Bit>(new LSR<Int>(instruction, 28));
        Short immVal = ExecTo<Short>(new LSR<Int>(instruction, 0));

        return (mode, opCode, dest, a, b, isImm, immVal);
    }
}
