using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Gates;

public class AND<T>(T inputA, T inputB) : IGate<T, T> where T : struct, IValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;

    public static implicit operator T(AND<T> gate)
    {
        var inputA = gate._inputA;
        var inputB = gate._inputB;
        var result = T.Zero;

        for (int i = 0; i < T.BitWidth; i++)
        {
            Bit bit = new SW<Bit>(
              inputA.GetBit(i),
              inputB.GetBit(i)
            );
            result.SetBit(i, bit);
        }

        return result;
    }
}