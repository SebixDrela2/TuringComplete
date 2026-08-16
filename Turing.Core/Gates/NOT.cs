using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Gates;

public class NOT<T>(T input) : TurComponentValue<T>, IGate<T, T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        var result = T.Zero;

        for (int i = 0; i < T.BitWidth; i++)
        {
            Bit bit = new NSW<Bit>(input.GetBit(i), Bit.One);
            result.SetBit(i, bit);
        }

        return result;
    }
}