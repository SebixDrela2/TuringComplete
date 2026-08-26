using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Symphony;

/// <summary>
/// <br>SYMPHONY counter, increments value by 4 and can be overriden</br>
/// </summary>
/// <typeparam name="T"></typeparam>
public class COUNTER<T> : TurComponentValue<T>, IStateGate<T> where T : struct, IValue<T>
{
    private readonly REGISTER<T> _register;
    public readonly CLOCK _clock;
    private readonly CONST<T> _one;

    public T State => _register.State;

    public COUNTER(CLOCK clock)
    {
        _clock = clock;
        _register = new REGISTER<T>(_clock);
        _one = new CONST<T>(4);
    }

    public COUNTER(T initialValue, CLOCK clock)
    {
        _clock = clock;
        _register = new REGISTER<T>(_clock);
        _one = new CONST<T>(4);
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

    protected override T ImplicitOperator() => State;

    private static T Add(T a, T b)
    {
        (T Sum, Bit Carry) result = new ADDER<T>(a, b, T.Zero);
        return result.Sum;
    }
}