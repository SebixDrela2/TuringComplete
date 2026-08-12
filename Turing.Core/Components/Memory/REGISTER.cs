using Turing.Core.Electricity;

namespace Turing.Core.Components.Memory;

public class REGISTER<T> : IStateGate<T> where T : struct, IBitValue<T>
{
    private readonly SLATCH<T> _slatch;
    private readonly DELAY<T> _delay;

    private T _state = default(T).FromValue(false);

    public T State => _state;

    public REGISTER()
    {
        _slatch = new SLATCH<T>();
        _delay = new DELAY<T>();
    }

    public REGISTER(T initialValue)
    {
        _state = initialValue;
        _slatch = new SLATCH<T>();
        _delay = new DELAY<T>();
    }

    public void EVal(Bit set, T input, Bit Tick)
    {
        _slatch.EVal(input, set);
        var latched = (T)_slatch;

        _delay.EVal(latched, Tick);
        _state = (T)_delay;
    }

    public static implicit operator T(REGISTER<T> register)
    {
        return register._state;
    }

    public void Reset()
    {
        _state = default(T).FromValue(false);
        _slatch.Reset();
        _delay.Reset();
    }
}
