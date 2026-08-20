using Turing.Core.Components.Memory;
using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

/// <summary>
/// <br>REGISTER is a core of digital logic/memory it is THE component for saving and loading values from.</br>
/// <br>REGISTER on save, saves value with one tick DELAY as it requires DELAY component.</br>
/// <br>REGISTER always outputs current tick value implictly.</br>
/// <br>REGISTER requires outside clock, provided for DELAY.</br>
/// </summary>
/// <typeparam name="T"></typeparam>
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

    public void EVal(Bit save, T input)
    {
        _slatch.EVal(input, save);
        var latched = (T)_slatch;

        _delay.EVal(latched);
    }

    public void Reset()
    {
        _delay.Reset();
    }

    protected override T ImplicitOperator() => State;
}