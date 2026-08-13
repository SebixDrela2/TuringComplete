using Turing.Core.Components.Logic;
using Turing.Core.Electricity;

namespace Turing.Core.Components.Memory;

public class SLATCH<T> : IStateGate<T> where T : struct, IBitValue<T>
{
    private T _state = default(T).FromValue(false);

    public T State => _state;

    public SLATCH() { }

    public SLATCH(T input, Bit set)
    {
        EVal(input, set);
    }

    public void EVal(T input, Bit set)
    {
        var mux = new MUX<T>(_state, input, set);

        _state = (T)mux;
    }

    public static implicit operator T(SLATCH<T> latch)
    {
        return latch._state;
    }

    public void Reset()
    {
        _state = default(T).FromValue(false);
    }
}