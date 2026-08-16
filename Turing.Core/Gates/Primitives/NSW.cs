using Turing.Core.Components;
using Turing.Core.Electricity;

namespace Turing.Core.Gates.Primitives;

[Component(Primitive = true)]
public class NSW<T>(Bit gate, T source) : TurComponentValue<T>, ISW<T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        return !gate ? source : T.Zero;
    }
}