using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Logic;

public class MUX<T>(T inputA, T inputB, Bit sel) where T : struct, IValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;
    private readonly Bit _sel = sel;

    public static implicit operator T(MUX<T> mux)
    {
        Bit notSel = new NOT<Bit>(mux._sel);

        var notSelT = mux._inputA.FromValue(notSel.Value);
        var selT = mux._inputA.FromValue(mux._sel.Value);

        T and1 = new AND<T>(mux._inputA, notSelT);
        T and2 = new AND<T>(mux._inputB, selT);
        T result = new OR<T>(and1, and2);

        return result;
    }
}
