namespace Turing.Core.Electricity;

public record struct Byte : IByteValue<Byte>
{
    private bool[] _bits;
    public bool[] Bits => _bits ??= new bool[8];

    public Byte(bool[] bits)
    {
        if (bits.Length != 8) throw new ArgumentException("Must have 8 bits");
        _bits = bits;
    }

    public Byte(int value)
    {
        _bits = new bool[8];
        for (int i = 0; i < 8; i++)
        {
            _bits[i] = ((value >> i) & 1) == 1;
        }
    }

    public Byte Value => FromBits(Bits);
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
    public Bit GetBit(int index) => Bits[index];
    public Byte SetBit(int index, bool value)
    {
        var newBits = (bool[])Bits.Clone();
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
            if (b.Bits[i]) result |= 1 << i;
        return result;
    }

    public static implicit operator Short(Byte b)
    {
        var bits = new bool[16];
        for (int i = 0; i < 8; i++) bits[i] = b.Bits[i];
        return new Short(bits);
    }

    public static implicit operator Int(Byte b)
    {
        var bits = new bool[32];
        for (int i = 0; i < 8; i++) bits[i] = b.Bits[i];
        return new Int(bits);
    }

    public static implicit operator Long(Byte b)
    {
        var bits = new bool[64];
        for (int i = 0; i < 8; i++) bits[i] = b.Bits[i];
        return new Long(bits);
    }

    public override string ToString()
    {
        var result = 0;

        for (int i = 0; i < 8; i++)
        {
            if (Bits[i])
            {
                result |= 1 << i;
            }
        }

        return result.ToString();
    }

    public bool Equals(Byte other)
    {
        if (Bits.Length != other.Bits.Length) return false;
        for (int i = 0; i < Bits.Length; i++)
            if (Bits[i] != other.Bits[i]) return false;
        return true;
    }

    public override int GetHashCode()
    {
        int hash = 0;
        for (int i = 0; i < Bits.Length; i++)
            if (Bits[i]) hash |= 1 << i;
        return hash;
    }
    
    public string ToBinaryString() => string.Concat(Bits.Reverse().Select(b => b ? "1" : "0"));
    public string ToHexString() => ((int)this).ToString("X2");
}