using Turing.Core.Electricity;

namespace Turing.Core.Global;

public abstract class TurComponentValue
{
    public Y Into<Y>()
        where Y : struct, IValue<Y>
    {
        return Into<Y>(Y.BitWidth);
    }

    public abstract Y Into<Y>(int bitWidth)
        where Y : struct, IValue<Y>;
}

public abstract class TurComponentValue<T> : TurComponentValue
    where T : struct, IValue<T>
{
    public override Y Into<Y>(int bitWidth) => ImplicitOperator().Into<Y>(bitWidth);

    public static implicit operator T(TurComponentValue<T> comp) => comp.ImplicitOperator();

    protected abstract T ImplicitOperator();

}

public abstract class TurComponent<T>
    where T : struct
{
    public static implicit operator T(TurComponent<T> comp) => comp.ImplicitOperator();

    protected abstract T ImplicitOperator();
}