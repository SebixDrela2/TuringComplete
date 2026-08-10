using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Gates;

public class NAND<T>(T inputA, T inputB) : IGate<T, T> where T : struct, IBitValue<T>
{
    private readonly SW<Bit> _t1 = new();
    private readonly SW<Bit> _t2 = new();

    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;

    public static implicit operator T(NAND<T> gate)
    {
        var resultBits = new bool[T.BitWidth];

        for (int i = 0; i < T.BitWidth; i++)
        {
            var aBit = new Bit((bool)gate._inputA.GetBit(i));
            var bBit = new Bit((bool)gate._inputB.GetBit(i));

            var t1Out = gate._t1.Eval(aBit, bBit);
            var t2Out = gate._t2.Eval(bBit, t1Out);

            resultBits[i] = !t2Out.Value;
        }

        return gate._inputA.FromBits(resultBits);
    }
}
