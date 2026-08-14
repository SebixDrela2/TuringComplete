using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Gates;

public class NAND<T>(T inputA, T inputB) : IGate<T, T> where T : struct, IValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;

    public static implicit operator T(NAND<T> gate)
    {
        var resultBits = new bool[T.BitWidth];

        for (int i = 0; i < T.BitWidth; i++)
        {
            var aBit = new Bit((bool)gate._inputA.GetBit(i));
            var bBit = new Bit((bool)gate._inputB.GetBit(i));

            Bit t1Out = new SW<Bit>(aBit, bBit);
            Bit t2Out = new SW<Bit>(bBit, t1Out);

            Bit notted = !t2Out;
            resultBits[i] = notted.Value;
        }

        return gate._inputA.FromBits(resultBits);
    }
}
