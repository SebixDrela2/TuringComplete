using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Components.Memory;

public class SLATCH<T> : IStateGate<T> where T : struct, IBitValue<T>
{
    private CLOCK _clock;
    private T _state = default(T).FromValue(false);

    public T State => _state;

    public SLATCH(CLOCK set) 
    {
        _clock = set;
    }

    public SLATCH(T input, CLOCK set)
    {
        _clock = set;

        EVal(input);
    }

    public void EVal(T input)
    {
        var mux = new MUX<T>(_state, input, _clock);

        _state = (T)mux;
    }

    public static implicit operator T(SLATCH<T> latch)
    {
        return latch.State;
    }

    public void Reset()
    {
        _state = default(T).FromValue(false);
    }
}