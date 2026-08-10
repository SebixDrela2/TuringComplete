using Turing.Core.Components.Logic;
using Turing.Core.Components.Memory;
using Turing.Core.Electricity;

public class COUNTER<T> : IStateGate<T> where T : struct, IBitValue<T>
{
    private T _state = default(T).FromValue(false);
    private readonly REGISTER<T> _register;
    private readonly CONST<T> _one;

    public T State => _state;

    public COUNTER()
    {
        _register = new REGISTER<T>();
        _state = default(T).FromValue(false);
        _one = new CONST<T>(1);
    }

    public COUNTER(T initialValue)
    {
        _register = new REGISTER<T>();
        _one = new CONST<T>(1);
        // Load initial value
        Load(initialValue);
    }

    public void EVal(Bit load, T loadValue, Bit tick)
    {
        T one = (T)_one;
        T incremented = Add(_state, one);
        T muxResult = new MUX<T>(incremented, loadValue, load);

        _register.EVal(new Bit(true), muxResult, tick);
        _state = (T)_register;
    }

    public void Load(T value)
    {
        _register.EVal(new Bit(true), value, new Bit(true));
        _state = (T)_register;
    }

    public void Reset()
    {
        _state = default(T).FromValue(false);
        _register.Reset();
    }

    public static implicit operator T(COUNTER<T> counter)
    {
        return counter._state;
    }

    private static T Add(T a, T b)
    {
        T zero = a.FromValue(false);
        (T Sum, Bit Carry) result = new ADDER<T>(a, b, zero);
        return result.Sum;
    }
}