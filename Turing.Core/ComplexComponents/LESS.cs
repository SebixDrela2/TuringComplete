using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.ComplexComponents;

public class LESS<T>(T inputA, T inputB) : TurComponentValue<T> where T : struct, IByteValue<T>
{
    protected override T ImplicitOperator()
    {
        (T _, Bit carry) = ((T, Bit)) new ADDER<T>(new NOT<T>(inputA), inputB, T.Zero);

        return carry;
    }
}
