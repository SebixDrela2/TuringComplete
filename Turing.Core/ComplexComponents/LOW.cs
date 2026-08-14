using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.ComplexComponents;

public class LOW<T>(T inputA, T inputB) where T : struct, IByteValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;

    public static implicit operator Bit(LOW<T> low)
    {
        (T _, Bit carry) = ((T, Bit)) new ADDER<T>(new NOT<T>(low._inputA), low._inputB, T.Zero);

        var nsignA = low._inputA.LastBit();
        var nsignB = low._inputB.LastBit();

        Bit xnor1 = new XNOR<Bit>(carry, nsignA);
        Bit xnor2 = new XNOR<Bit>(xnor1, nsignB);

        return xnor2;
    }
}
