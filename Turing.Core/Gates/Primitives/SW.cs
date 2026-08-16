using Turing.Core.Components;
using Turing.Core.Electricity;

namespace Turing.Core.Gates.Primitives;

public interface ISW<T> where T : struct, IValue<T>;

[Component(Primitive = true)]
public class SW<T>(Bit gate, T source) : ISW<T> where T : struct, IValue<T>
{
    private readonly Bit _gate = gate;
    private readonly T _source = source;

    public static implicit operator T(SW<T> sw)
    {
        return sw._gate.Value ? sw._source : sw._source.FromValue(false);
    }
}
