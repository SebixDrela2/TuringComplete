using Turing.Core.Components;
using Turing.Core.Electricity;

namespace Turing.Core.Gates.Primitives;

public interface ISW<T> where T : struct, IValue<T>;

/// <summary>
/// <br>Switch, profesionally called PN transistor is the most primitive "gate"</br>
/// <br>It outputs source if gate is ON and in every other case OFF</br>
/// <br>This is one of two components using C# conditionals to simulate current in this project.</br>
/// </summary>
/// <typeparam name="T"></typeparam>
[Component(Primitive = true)]
public class SW<T>(Bit gate, T source) : TurComponentValue<T>, ISW<T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        return gate.Value ? source : T.Zero;
    }
}
