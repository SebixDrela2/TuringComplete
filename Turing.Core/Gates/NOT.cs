using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Gates;

public class NOT<T>(T input) : IGate<T, T> where T : struct, IValue<T>
{
    private readonly T _input = input;

    public static implicit operator T(NOT<T> gate)
    {
        var input = gate._input;
        var result = T.Zero;

        for (int i = 0; i < T.BitWidth; i++)
        {
            Bit bit = new NSW<Bit>(input.GetBit(i), Bit.One);
            result.SetBit(i, bit);
        }

        return result;
    }
}