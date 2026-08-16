using Turing.Core.ComplexComponents;
using Turing.Core.Electricity;

namespace Turing.Core.Computer.Symphony;

internal class INSTRUCTION_DECODER(Int instruction)
{
    private readonly Int _instruction = instruction;

    public static implicit operator(Byte Mode, Byte OpCode, Byte Destination, Byte A, Byte B, Bit IsImm, Short ImmVal)(INSTRUCTION_DECODER decoder)
    {
        Byte mode = ExecTo<Byte>(new LSR<Int>(decoder._instruction, 29));
        Byte opCode = ExecTo<Byte>(new LSR<Int>(decoder._instruction, 24));
        Byte dest = ExecTo<Byte>(new LSR<Int>(decoder._instruction, 20));
        Byte a = ExecTo<Byte>(new LSR<Int>(decoder._instruction, 16));
        Byte b = ExecTo<Byte>(new LSR<Int>(decoder._instruction, 8));
        Bit isImm = ExecTo<Bit>(new LSR<Int>(decoder._instruction, 28));
        Short immVal = ExecTo<Short>(new LSR<Int>(decoder._instruction, 0));

        return (mode, opCode, dest, a, b, isImm, immVal);
    }
}
