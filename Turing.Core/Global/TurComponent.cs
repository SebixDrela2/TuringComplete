using Turing.Core.Electricity;

namespace Turing.Core.Global;

public abstract class TurComponent
{
    public abstract Y Into<Y>()
        where Y : struct, IValue<Y>;
}

public abstract class TurComponent<T> : TurComponent
    where T : struct, IValue<T>
{

    public T Value => this;

    public override Y Into<Y>() => Value.Into<Y>();

    public static implicit operator T(TurComponent<T> comp) => comp.ImplicitOperator();

    protected abstract T ImplicitOperator();

}