using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

public class COUNTER_OVERTURE<T> : IStateGate<T> where T : struct, IValue<T>
{
    private readonly REGISTER<T> _register;
    public readonly CLOCK _clock;
    private readonly CONST<T> _one;

    public T State => _register.State; 

    public COUNTER_OVERTURE(CLOCK clock)
    {
        _clock = clock;
        _register = new REGISTER<T>(_clock);
        _one = new CONST<T>(1);
    }

    public COUNTER_OVERTURE(T initialValue, CLOCK clock)
    {
        _clock = clock;
        _register = new REGISTER<T>(_clock);
        _one = new CONST<T>(1);
        Load(initialValue);
    }

    public void EVal(Bit load, T loadValue)
    {
        T currentState = _register.State;
        T one = (T)_one;
        T incremented = Add(currentState, one);
        T muxResult = new MUX<T>(incremented, loadValue, load);

        _register.EVal(_clock, muxResult);
    }

    public void Load(T value)
    {
        _register.EVal(new Bit(true), value);
    }

    public void Reset()
    {
        _register.Reset();
    }

    public static implicit operator T(COUNTER_OVERTURE<T> counter)
    {
        return counter.State;
    }

    private static T Add(T a, T b)
    {
        T zero = a.FromValue(false);
        (T Sum, Bit Carry) result = new ADDER<T>(a, b, zero);
        return result.Sum;
    }
}