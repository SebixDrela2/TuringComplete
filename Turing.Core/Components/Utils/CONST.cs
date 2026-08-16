using Turing.Core.Electricity;

namespace Turing.Core.Components.Logic;

[Component(Primitive = true)]
public class CONST<T> where T : struct, IValue<T>
{
    private readonly T _value;

    public CONST(ulong value)
    {
        var bits = new bool[T.BitWidth];

        for (int i = 0; i < T.BitWidth && i < 64; i++)
        {
            bits[i] = ((value >> i) & 1) == 1;
        }

        T template = default(T);
        _value = template.FromBits(bits);
    }

    public static implicit operator T(CONST<T> constant)
    {
        return constant._value;
    }
}