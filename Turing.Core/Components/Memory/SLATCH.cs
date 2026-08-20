using Turing.Core.Components.Logic;
using Turing.Core.Electricity;

namespace Turing.Core.Components.Memory;

/// <summary>
/// <br>SLATCH commonly known as set latch is a component which locks in value on high set</br>
/// <br>This is an absolute base for digital logic memory.</br>
/// <br>It is also first component which has a state on it's own.</br>
/// </summary>
/// <typeparam name="T"></typeparam>
public class SLATCH<T> : TurComponentValue<T>, IStateGate<T> where T : struct, IValue<T>
{
    private T _state = T.Zero;

    public T State => _state;

    public SLATCH() { }

    public SLATCH(T input, Bit set)
    {
        EVal(input, set);
    }

    public void EVal(T input, Bit set)
    {
        var mux = new MUX<T>(_state, input, set);

        _state = (T)mux;
    }

    public void Reset()
    {
        _state = T.Zero;
    }

    protected override T ImplicitOperator() => _state;
}