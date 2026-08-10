namespace Turing.Core.Electricity;

public readonly record struct Short : IBitValue<Short>
{
    private readonly bool[] _bits; 

    public Short(bool[] bits)
    {
        if (bits.Length != 16) throw new ArgumentException("Must have 16 bits");
        _bits = bits;
    }

    public Short(int value)
    {
        _bits = new bool[16];
        for (int i = 0; i < 16; i++)
            _bits[i] = ((value >> i) & 1) == 1;
    }

    public bool Value => _bits[0];
    public int BitWidth => 16;

    public Short FromValue(bool value)
    {
        if (value)
        {
            // Return all ones (0xFFFF)
            var bits = new bool[16];
            for (int i = 0; i < 16; i++) bits[i] = true;
            return new Short(bits);
        }

        return new Short(0);
    }

    public Short FromBits(bool[] bits) => new Short(bits);

    public Bit GetBit(int index) => _bits[index];
    public Short SetBit(int index, bool value)
    {
        var newBits = (bool[])_bits.Clone();
        newBits[index] = value;
        return new Short(newBits);
    }

    public static implicit operator Short(int value)
    {
        var bits = new bool[16];
        for (int i = 0; i < 16; i++)
            bits[i] = ((value >> i) & 1) == 1;
        return new Short(bits);
    }

    public static implicit operator int(Short s)
    {
        int result = 0;
        for (int i = 0; i < 16; i++)
            if (s._bits[i]) result |= 1 << i;
        return result;
    }

    public static implicit operator Int(Short s)
    {
        var bits = new bool[32];
        for (int i = 0; i < 16; i++)
            bits[i] = s._bits[i];
        return new Int(bits);
    }

    public static implicit operator Long(Short s)
    {
        var bits = new bool[64];
        for (int i = 0; i < 16; i++)
            bits[i] = s._bits[i];
        return new Long(bits);
    }

    public static explicit operator Short(Int i)
    {
        var bits = new bool[16];
        for (int x = 0; x < 16; x++)
            bits[x] = i.GetBit(x);
        return new Short(bits);
    }

    public override string ToString()
    {
        int result = 0;
        for (int i = 0; i < 16; i++)
            if (_bits[i]) result |= 1 << i;
        return result.ToString();
    }

    public string ToBinaryString()
        => string.Concat(_bits.Reverse().Select(b => b ? "1" : "0"));

    public string ToHexString()
        => ((int)this).ToString("X4");

    public bool Equals(Short other)
    {
        return ((ReadOnlySpan<bool>)_bits).SequenceEqual((ReadOnlySpan<bool>)other._bits);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        foreach (var bit in _bits)
            hashCode.Add(bit);
        return hashCode.ToHashCode();
    }
}
