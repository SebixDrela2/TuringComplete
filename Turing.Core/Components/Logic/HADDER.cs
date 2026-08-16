using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Logic;

public class HADDER<T>(T inputA, T inputB) : TurComponent<(T Sum, Bit Carry)> where T : struct, IValue<T>
{
    protected override (T Sum, Bit Carry) ImplicitOperator()
    {
        T sum = new XOR<T>(inputA, inputB);
        T carry = new AND<T>(inputA, inputB);

        return (sum, carry);
    }
}

