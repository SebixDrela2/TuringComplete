using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Memory;

public class DELAY<T> : IStateGate<T> where T : struct, IBitValue<T>
{
    private T _state = default(T).FromValue(false);
    private SLATCH<T> _masterLatch;
    private SLATCH<T> _slaveLatch;

    public T State => _state;

    public DELAY()
    {
        _masterLatch = new SLATCH<T>();
        _slaveLatch = new SLATCH<T>();
        _masterLatch.EVal(default(T).FromValue(false), new Bit(true));
        _slaveLatch.EVal(default(T).FromValue(false), new Bit(true));
        _state = default(T).FromValue(false);
    }

    public DELAY(T input, Bit tick)
    {
        _masterLatch = new SLATCH<T>();
        _slaveLatch = new SLATCH<T>();
        _masterLatch.EVal(default(T).FromValue(false), new Bit(true));
        _slaveLatch.EVal(default(T).FromValue(false), new Bit(true));
        _state = default(T).FromValue(false);
        EVal(input, tick);
    }

    public void EVal(T input, Bit tick)
    {
        var notTick = new NOT<Bit>(tick);
        var negTick = (Bit)notTick;

        _masterLatch.EVal(input, tick);
        var masterState = (T)_masterLatch;

        _slaveLatch.EVal(masterState, negTick);
        var slaveState = (T)_slaveLatch;

        var mux = new MUX<T>(slaveState, masterState, tick);
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
        _masterLatch.EVal(default(T).FromValue(false), new Bit(true));
        _slaveLatch.EVal(default(T).FromValue(false), new Bit(true));
    }
}