using Turing.Core.Electricity;

namespace Turing.Core.Gates.Primitives;

public class NSW<T> : ISW<T> where T : struct, IBitValue<T>
{
    public T Eval(T gate, T source)
    {
        return !gate.Value ? source : source.FromValue(false);
    }
}