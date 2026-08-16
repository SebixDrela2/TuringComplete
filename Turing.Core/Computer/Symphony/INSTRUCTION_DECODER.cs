using Turing.Core.ComplexComponents;
using Turing.Core.Electricity;

namespace Turing.Core.Computer.Symphony;

internal class INSTRUCTION_DECODER(Int instruction)
{
    private readonly Int _instruction = instruction;

    public static implicit operator(Byte Mode, Byte OpCode, Byte Destination, Byte A, Byte B, Bit IsImm, Short ImmVal)(INSTRUCTION_DECODER decoder)
    {
        Byte mode = ((Int)new SINDEXER<Int>(decoder._instruction, 29)).Into<Byte>();
        Byte opCode = ((Int)new SINDEXER<Int>(decoder._instruction, 24)).Into<Byte>();
        Byte dest = ((Int)new SINDEXER<Int>(decoder._instruction, 20)).Into<Byte>();
        Byte a = ((Int)new SINDEXER<Int>(decoder._instruction, 16)).Into<Byte>();
        Byte b = ((Int)new SINDEXER<Int>(decoder._instruction, 8)).Into<Byte>();
        Bit isImm = ((Int)new SINDEXER<Int>(decoder._instruction, 28)).Into<Bit>();
        Short immVal = ((Int)new SINDEXER<Int>(decoder._instruction, 0)).Into<Short>();

        return (mode, opCode, dest, a, b, isImm, immVal);
    }
}
