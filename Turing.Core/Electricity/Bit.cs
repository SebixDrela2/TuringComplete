namespace Turing.Core.Electricity;

public readonly record struct Bit : IBitValue<Bit>
{
    private readonly bool _value;

    public Bit(bool value) => _value = value;
    public Bit(int value) => _value = value == 1;

    public Bit Value => _value;
    public static int BitWidth => 1;

    public Bit FromValue(bool value) => new(value);
    public Bit FromBits(bool[] bits) => bits.Length > 0 ? new Bit(bits[0]) : new Bit(false);
    public Bit GetBit(int index) => index == 0 ? _value : throw new IndexOutOfRangeException();
    public Bit SetBit(int index, bool value) => index == 0 ? new Bit(value) : throw new IndexOutOfRangeException();

    public static implicit operator Bit(bool value) => new(value);
    public static implicit operator bool(Bit bit) => bit._value;
    public static implicit operator Bit(int value) => new(value != 0);
    public static implicit operator int(Bit bit) => bit._value ? 1 : 0;

    public static implicit operator Byte(Bit bit)
    {
        var bits = new bool[8];
        bits[0] = bit._value;
        return new Byte(bits);
    }

    // Same for Short, Int, Long
    public static implicit operator Short(Bit bit)
    {
        var bits = new bool[16];
        bits[0] = bit._value;
        return new Short(bits);
    }

    public static implicit operator Int(Bit bit)
    {
        var bits = new bool[32];
        bits[0] = bit._value;
        return new Int(bits);
    }

    public static implicit operator Long(Bit bit)
    {
        var bits = new bool[64];
        bits[0] = bit._value;
        return new Long(bits);
    }

    public override string ToString() => _value ? "1" : "0";
    public string ToBinaryString() => ToString();

    public bool Equals(Bit other)
    {
        return _value == other._value;
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
}