using Turing.Core.Electricity;
using Turing.Core.Gates;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Components.Memory;

public class DELAY<T> : TurComponentValue<T>, IStateGate<T> where T : struct, IValue<T>
{
    private readonly CLOCK _clock;
    private SLATCH<T> _masterLatch;
    private SLATCH<T> _slaveLatch;

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