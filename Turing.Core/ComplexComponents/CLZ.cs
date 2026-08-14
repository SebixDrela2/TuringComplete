using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.ComplexComponents;

public class CLZ<T>(T input) where T : struct, IByteValue<T>
{
    private readonly T _input = input;

    public static implicit operator T(CLZ<T> clz)
    {
        (T Val, Bit _) result = ((T, Bit))new ADDER<T>(T.Zero, T.Zero, T.One);

        T notInput = new NOT<T>(clz._input);

        var swBit = notInput.GetBit(0);

        result.Val = new SW<T>(swBit, result.Val);

        for (var i = 1; i < T.BitWidth; i++)
        {
            result = ((T, Bit)) new ADDER<T>(result.Val, T.One, T.Zero);
            var nextMinusBit = notInput.GetBit(i);

            result.Val = new SW<T>(nextMinusBit, result.Val);
        }

        return result.Val;
    }
}

