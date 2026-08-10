using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Logic;

public class HADDER<T>(T inputA, T inputB) where T : struct, IBitValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;

    public static implicit operator (T Sum, T Carry)(HADDER<T> adder)
    {
        T sum = new XOR<T>(adder._inputA, adder._inputB);
        T carry = new AND<T>(adder._inputA, adder._inputB);

        return (sum, carry);
    }
}

