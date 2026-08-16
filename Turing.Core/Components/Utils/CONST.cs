using Turing.Core.Electricity;

namespace Turing.Core.Components.Logic;

[Component(Primitive = true)]
public class CONST<T> : TurComponentValue<T> where T : struct, IValue<T>
{
    private readonly T _value;

    public CONST(ulong value)
    {
        var bits = new bool[T.BitWidth];

        for (int i = 0; i < T.BitWidth; i++)
        {
            bits[i] = ((value >> i) & 1) == 1;
        }

        _value = T.FromBits(bits);
    }

    protected override T ImplicitOperator() => _value;
}