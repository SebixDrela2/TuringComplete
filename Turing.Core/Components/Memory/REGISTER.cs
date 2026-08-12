using Turing.Core.Components.Logic;
using Turing.Core.Components.Memory;
using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

public class REGISTER<T> : IStateGate<T> where T : struct, IBitValue<T>
{
    private readonly DELAY<T> _delay;

    public T State => _delay;                

    public REGISTER(CLOCK clock)
    {
        _delay = new DELAY<T>(clock);
    }

    public void EVal(Bit set, T input)
    {
        T muxInput = new MUX<T>(_delay, input, set);

        _delay.EVal(muxInput);              
    }

    public static implicit operator T(REGISTER<T> register) => register.State;

    public void Reset()
    {
        _delay.Reset();
    }
}