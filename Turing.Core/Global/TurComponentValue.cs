using Turing.Core.Electricity;

namespace Turing.Core.Global;

public abstract class TurComponentValue
{
    public abstract Y Into<Y>()
        where Y : struct, IValue<Y>;
}

public abstract class TurComponentValue<T> : TurComponentValue
    where T : struct, IValue<T>
{
    public override Y Into<Y>() => ImplicitOperator().Into<Y>();

    public static implicit operator T(TurComponentValue<T> comp) => comp.ImplicitOperator();

    protected abstract T ImplicitOperator();

}

public abstract class TurComponent<T>
    where T : struct
{
    public static implicit operator T(TurComponent<T> comp) => comp.ImplicitOperator();

    protected abstract T ImplicitOperator();
}