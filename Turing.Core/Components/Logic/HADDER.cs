using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Logic;

public class HADDER(Bit inputA, Bit inputB) : TurComponent<(Bit Sum, Bit Carry)>
{
    protected override (Bit Sum, Bit Carry) ImplicitOperator()
    {
        Bit sum = new XOR<Bit>(inputA, inputB);
        Bit carry = new AND<Bit>(inputA, inputB);

        return (sum, carry);
    }
}

