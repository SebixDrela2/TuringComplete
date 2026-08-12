namespace Turing.Core.Gates.Primitives;

public class CLOCK
{

    private Bit _tick;
    public Bit TickVal => _tick;

    public void Tick()
    {
        _tick = new NOT<Bit>(_tick);
    }

    public void Set(Bit bit) => _tick = bit;

    public static implicit operator Bit(CLOCK clock) => clock._tick;
}
