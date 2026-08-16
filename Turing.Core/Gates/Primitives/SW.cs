using Turing.Core.Components;
using Turing.Core.Electricity;

namespace Turing.Core.Gates.Primitives;

public interface ISW<T> where T : struct, IValue<T>;

[Component(Primitive = true)]
public class SW<T>(Bit gate, T source) : TurComponentValue<T>, ISW<T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        return gate.Value ? source : T.Zero;
    }
}
