using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.ComplexComponents;

public class LOW<T>(T inputA, T inputB) : TurComponentValue<Bit> where T : struct, IByteValue<T>
{
    private readonly T inputA = inputA;
    private readonly T inputB = inputB;

    protected override Bit ImplicitOperator()
    {
        (T _, Bit carry) = ((T, Bit)) new ADDER<T>(new NOT<T>(inputA), inputB, T.Zero);

        var nsignA = inputA.LastBit();
        var nsignB = inputB.LastBit();

        Bit xnor1 = new XNOR<Bit>(carry, nsignA);
        Bit xnor2 = new XNOR<Bit>(xnor1, nsignB);

        return xnor2;
    }
}
