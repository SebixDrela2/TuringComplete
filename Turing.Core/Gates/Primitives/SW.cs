using Turing.Core.Electricity;

namespace Turing.Core.Gates.Primitives;

public interface ISW<T> where T : struct, IBitValue<T>
{
    T Eval(T gate, T source);
}

public class SW<T> : ISW<T> where T : struct, IBitValue<T>
{
    public T Eval(T gate, T source)
    {
        return gate.Value ? source : source.FromValue(false);
    }
}
