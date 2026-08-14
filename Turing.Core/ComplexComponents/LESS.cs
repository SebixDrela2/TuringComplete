using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.ComplexComponents;

public class LESS<T>(T inputA, T inputB) where T : struct, IByteValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;

    public static implicit operator Bit(LESS<T> less)
    {
        (T _, Bit carry) = ((T, Bit)) new ADDER<T>(new NOT<T>(less._inputA), less._inputB, T.Zero);

        return carry;
    }
}
