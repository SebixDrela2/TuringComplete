using Turing.Core.Components;
using Turing.Core.Electricity;

namespace Turing.Core.Gates.Primitives;

[Component(Primitive = true)]
public class NSW<T>(Bit gate, T source) : ISW<T> where T : struct, IValue<T>
{
    private readonly Bit _gate = gate;
    private readonly T _source = source;

    public static implicit operator T(NSW<T> nsw)
    {
        return !nsw._gate ? nsw._source : nsw._source.FromValue(false);
    }
}