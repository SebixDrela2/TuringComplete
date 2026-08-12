namespace Turing.Core.Electricity;

public readonly record struct Long : IBitValue<Long>
{
    private readonly bool[] _bits;

    public Long(bool[] bits)
    {
        if (bits.Length != 64) throw new ArgumentException("Must have 64 bits");
        _bits = bits;
    }

    public Long(long value)
    {
        _bits = new bool[64];
        for (int i = 0; i < 64; i++)
            _bits[i] = ((value >> i) & 1) == 1;
    }

    public Long(ulong value)
    {
        _bits = new bool[64];
        for (int i = 0; i < 64; i++)
            _bits[i] = ((value >> i) & 1) == 1;
    }

    public Long Value => FromBits(_bits);
    public static int BitWidth => 64;

    public Long FromValue(bool value)
    {
        if (value)
        {
            // Return all ones (0xFFFFFFFFFFFFFFFF)
            var bits = new bool[64];
            for (int i = 0; i < 64; i++) bits[i] = true;
            return new Long(bits);
        }

        return new Long(0);
    }
    public Long FromBits(bool[] bits) => new Long(bits);
    public Bit GetBit(int index) => _bits[index];
    public Long SetBit(int index, bool value)
    {
        var newBits = (bool[])_bits.Clone();
        newBits[index] = value;
        return new Long(newBits);
    }

    public static implicit operator Long(long value)
    {
        var bits = new bool[64];
        for (int i = 0; i < 64; i++)
            bits[i] = ((value >> i) & 1) == 1;
        return new Long(bits);
    }

    public static implicit operator long(Long l)
    {
        long result = 0;
        for (int x = 0; x < 64; x++)
            if (l._bits[x]) result |= 1L << x;
        return result;
    }

    public static implicit operator Long(ulong value)
    {
        var bits = new bool[64];
        for (int i = 0; i < 64; i++)
            bits[i] = ((value >> i) & 1) == 1;
        return new Long(bits);
    }

    public static implicit operator ulong(Long l)
    {
        ulong result = 0;
        for (int x = 0; x < 64; x++)
            if (l._bits[x]) result |= 1UL << x;
        return result;
    }

    public static explicit operator Int(Long l)
    {
        var bits = new bool[32];
        for (int x = 0; x < 32; x++)
            bits[x] = l.GetBit(x);
        return new Int(bits);
    }

    public static explicit operator Short(Long l)
    {
        var bits = new bool[16];
        for (int x = 0; x < 16; x++)
            bits[x] = l.GetBit(x);
        return new Short(bits);
    }

    public static explicit operator Byte(Long l)
    {
        var bits = new bool[8];
        for (int x = 0; x < 8; x++)
            bits[x] = l.GetBit(x);
        return new Byte(bits);
    }

    public static explicit operator Bit(Long l) => new Bit(l._bits[0]);

    public override string ToString()
    {
        ulong result = 0;
        for (int i = 0; i < 64; i++)
            if (_bits[i]) result |= 1UL << i;
        return result.ToString();
    }

    public string ToBinaryString()
        => string.Concat(_bits.Reverse().Select(b => b ? "1" : "0"));

    public string ToHexString()
        => ((ulong)this).ToString("X16");

    public bool Equals(Long other)
    {
        if (_bits.Length != other._bits.Length) return false;
        for (int i = 0; i < _bits.Length; i++)
            if (_bits[i] != other._bits[i]) return false;
        return true;
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        foreach (var bit in _bits)
            hashCode.Add(bit);
        return hashCode.ToHashCode();
    }

}
