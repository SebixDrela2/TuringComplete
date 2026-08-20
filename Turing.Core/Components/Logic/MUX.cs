using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Logic;

/// <summary>
/// <br>MUX is basically a choser, it takes three inputs.</br>
/// <br>If select bit is OFF it takes first value, otherwise if its ON it takes second value.</br>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="inputA"></param>
/// <param name="inputB"></param>
/// <param name="sel"></param>
public class MUX<T>(T inputA, T inputB, Bit sel) : TurComponentValue<T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        Bit notSel = new NOT<Bit>(sel);

        var notSelT = T.FromValue(notSel.Value);
        var selT = T.FromValue(sel.Value);

        T and1 = new AND<T>(inputA, notSelT);
        T and2 = new AND<T>(inputB, selT);
        T result = new OR<T>(and1, and2);

        return result;
    }
}
