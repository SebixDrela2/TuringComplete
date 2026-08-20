using Turing.Core.Electricity;
using Turing.Core.Gates;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Components.Memory;

/// <summary>
/// <br>DELAY commonly known as D flip flop delays it's current by one tick.</br>
/// <br>This is a first component which requires outside clock.</br>
/// <br>DELAY changes it's value on the rising edge, outputting previous state on current input and on next tick saved value.</br>
/// </summary>
/// <typeparam name="T"></typeparam>
public class DELAY<T> : TurComponentValue<T>, IStateGate<T> where T : struct, IValue<T>
{
    private readonly CLOCK _clock;
    private readonly SLATCH<T> _masterLatch;
    private readonly SLATCH<T> _slaveLatch;

    public T State => _slaveLatch;

    public DELAY(CLOCK clock)
    {
        _clock = clock;
        _masterLatch = new SLATCH<T>();
        _slaveLatch = new SLATCH<T>();
    }

    public void EVal(T input)
    {
        Bit tick = _clock;
        Bit negTick = new NOT<Bit>(_clock);

        _masterLatch.EVal(input, tick);
        _slaveLatch.EVal(_masterLatch, negTick);
    }
    protected override T ImplicitOperator()
    {
        return _slaveLatch;
    }

    public void Reset()
    {
        _masterLatch.Reset();
        _slaveLatch.Reset();
    }
}