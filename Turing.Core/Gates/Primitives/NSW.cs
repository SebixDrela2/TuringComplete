using Turing.Core.Components;
using Turing.Core.Electricity;

namespace Turing.Core.Gates.Primitives;

/// <summary>
/// <br>Negative switch, profesionally called NPN transistor is the most primitive "gate"</br>
/// <br>It outputs source if gate is OFF and in every other case OFF</br>
/// <br>This is one of two components using C# conditionals to simulate current in this project.</br>
/// </summary>
/// <typeparam name="T"></typeparam>
[Component(Primitive = true)]
public class NSW<T>(Bit gate, T source) : TurComponentValue<T>, ISW<T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        return !gate ? source : T.Zero;
    }
}