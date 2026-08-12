using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Components.Memory;

public class DELAY<T> : IStateGate<T> where T : struct, IBitValue<T>
{
    private readonly CLOCK _clock;
    private T _state = default(T).FromValue(false);
    private SLATCH<T> _masterLatch;
    private SLATCH<T> _slaveLatch;
    public T State => _state;

    public DELAY(CLOCK clock)
    {
        _clock = clock;
        _masterLatch = new SLATCH<T>(_clock);
        _slaveLatch = new SLATCH<T>(_clock);
        _masterLatch.EVal(default(T).FromValue(false));
        _slaveLatch.EVal(default(T).FromValue(false));
        _state = default(T).FromValue(false);
    }

    public DELAY(T input, CLOCK clock)
    {
        _clock = clock;
        _masterLatch = new SLATCH<T>(_clock);
        _slaveLatch = new SLATCH<T>(_clock);
        _masterLatch.EVal(default(T).FromValue(false));
        _slaveLatch.EVal(default(T).FromValue(false));
        _state = default(T).FromValue(false);

        EVal(input);
    }

    public void EVal(T input)
    {
        var notTick = new NOT<Bit>(_clock);
        var negTick = (Bit)notTick;

        _masterLatch.EVal(input);
        T masterState = _masterLatch;

        _slaveLatch.EVal(masterState);
        var slaveState = (T)_slaveLatch;

        var mux = new MUX<T>(slaveState, masterState, _clock);
        _state = (T)mux;
    }

    public static implicit operator T(DELAY<T> delay)
    {
        return delay._state;
    }

    public void Reset()
    {
        _state = default(T).FromValue(false);
        _masterLatch.Reset();
        _slaveLatch.Reset();
        _clock.Set(new Bit(true));
        _masterLatch.EVal(default(T).FromValue(false));
        _slaveLatch.EVal(default(T).FromValue(false));
    }
}