using Turing.Core.Components.Memory;
using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

public class REGISTER<T> : TurComponentValue<T>, IStateGate<T> where T : struct, IValue<T>
{
    private readonly SLATCH<T> _slatch;
    private readonly DELAY<T> _delay;

    public T State => _delay;                

    public REGISTER(CLOCK clock)
    {
        _slatch = new SLATCH<T>();
        _delay = new DELAY<T>(clock);
    }

    public void EVal(Bit set, T input)
    {
        _slatch.EVal(input, set);
        var latched = (T)_slatch;

        _delay.EVal(latched);
    }

    public void Reset()
    {
        _delay.Reset();
    }

    protected override T ImplicitOperator() => State;
}