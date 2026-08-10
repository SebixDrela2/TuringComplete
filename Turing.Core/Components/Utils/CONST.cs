using Turing.Core.Electricity;

namespace Turing.Core.Components.Logic;

public class CONST<T> where T : struct, IBitValue<T>
{
    private readonly T _value;

    public CONST(ulong value)
    {
        int bitWidth = GetBitWidth();
        var bits = new bool[bitWidth];

        for (int i = 0; i < bitWidth && i < 64; i++)
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

    private static int GetBitWidth()
    {
        return typeof(T) switch
        {
            Type t when t == typeof(Bit) => 1,
            Type t when t == typeof(Byte) => 8,
            Type t when t == typeof(Short) => 16,
            Type t when t == typeof(Int) => 32,
            Type t when t == typeof(Long) => 64,
            _ => throw new NotSupportedException($"Type {typeof(T)} not supported")
        };
    }
}