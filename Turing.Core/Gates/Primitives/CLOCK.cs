using Turing.Core.Components;

namespace Turing.Core.Gates.Primitives;

[Component(Primitive = true)]
public class CLOCK
{
    private Bit _tick = new Bit(1);
    public Bit TickVal => _tick;

    public void Tick()
    {
        _tick = new NOT<Bit>(_tick);
    }

    public void Set(Bit bit) => _tick = bit;

    public static implicit operator Bit(CLOCK clock) => clock._tick;
}
