namespace Turing.Core.Electricity;

/// <summary>
/// <br>Everything begins with a signal either low or high.</br>
/// <br>Low voltage represents no state, High voltage represents existing state."</br>
/// <br>In computer science its abstracted to a concept called "Bit".</br>
/// </summary>
public record struct Bit : IBitValue<Bit>
{
    private bool _value;

    public Bit(bool value) => _value = value;
    public Bit(int value) => _value = value == 1;

    public Bit Value => _value;

    public bool[] ToBits() => [_value];

    public static Bit Zero => 0;
    public static Bit One => 1;
    public static int BitWidth => 1;

    public static Bit FromValue(bool value) => new(value);
    public static Bit FromBits(bool[] bits) => bits.Length > 0 ? new Bit(bits[0]) : new Bit(false);
    public Bit GetBit(int index) => index == 0 ? _value : throw new IndexOutOfRangeException();
    public void SetBit(int index, bool value)
    {
        _value = value ? true : false;
    }

    public static implicit operator Bit(bool value) => new(value);
    public static implicit operator bool(Bit bit) => bit._value;
    public static implicit operator Bit(int value) => new(value != 0);
    public static implicit operator int(Bit bit) => bit._value ? 1 : 0;

#pragma warning disable CS0473 // Explicit interface implementation matches more than one interface member
    static implicit IValue<Bit>.operator Bit(Bit value) => value;
#pragma warning restore CS0473 // Explicit interface implementation matches more than one interface member

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