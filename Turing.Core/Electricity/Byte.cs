namespace Turing.Core.Electricity;

public readonly record struct Byte : IBitValue<Byte>
{
    private readonly bool[] _bits;
    public readonly bool[] Bits => _bits;

    public Byte(bool[] bits)
    {
        if (bits.Length != 8) throw new ArgumentException("Must have 8 bits");
        _bits = bits;
    }

    public Byte(int value)
    {
        _bits = new bool[8];
        for (int i = 0; i < 8; i++)
            _bits[i] = ((value >> i) & 1) == 1;
    }

    public bool Value => _bits[0];
    public static int BitWidth => 8;

    public Byte FromValue(bool value)
    {
        if (value)
        {
            // Return all ones (0xFF)
            var bits = new bool[8];
            for (int i = 0; i < 8; i++) bits[i] = true;
            return new Byte(bits);
        }

        return new Byte(0);
    }
    public Byte FromBits(bool[] bits) => new Byte(bits);
    public Bit GetBit(int index) => _bits[index];
    public Byte SetBit(int index, bool value)
    {
        var newBits = (bool[])_bits.Clone();
        newBits[index] = value;
        return new Byte(newBits);
    }

    public static implicit operator Byte(int value)
    {
        var bits = new bool[8];
        for (int i = 0; i < 8; i++)
            bits[i] = ((value >> i) & 1) == 1;
        return new Byte(bits);
    }

    public static implicit operator int(Byte b)
    {
        int result = 0;
        for (int i = 0; i < 8; i++)
            if (b._bits[i]) result |= 1 << i;
        return result;
    }

    public static implicit operator Short(Byte b)
    {
        var bits = new bool[16];
        for (int i = 0; i < 8; i++) bits[i] = b._bits[i];
        return new Short(bits);
    }

    public static implicit operator Int(Byte b)
    {
        var bits = new bool[32];
        for (int i = 0; i < 8; i++) bits[i] = b._bits[i];
        return new Int(bits);
    }

    public static implicit operator Long(Byte b)
    {
        var bits = new bool[64];
        for (int i = 0; i < 8; i++) bits[i] = b._bits[i];
        return new Long(bits);
    }

    public override string ToString()
    {
        int result = 0;
        for (int i = 0; i < 8; i++)
            if (_bits[i]) result |= 1 << i;
        return result.ToString();
    }

    public bool Equals(Byte other)
    {
        if (_bits.Length != other._bits.Length) return false;
        for (int i = 0; i < _bits.Length; i++)
            if (_bits[i] != other._bits[i]) return false;
        return true;
    }

    public override int GetHashCode()
    {
        int hash = 0;
        for (int i = 0; i < _bits.Length; i++)
            if (_bits[i]) hash |= 1 << i;
        return hash;
    }

    public string ToBinaryString() => string.Concat(_bits.Reverse().Select(b => b ? "1" : "0"));
    public string ToHexString() => ((int)this).ToString("X2");
}