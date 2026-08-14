namespace Turing.Core.Electricity;

public readonly record struct Int : IByteValue<Int>
{
	private readonly bool[] _bits; 

	public Int(bool[] bits)
	{
		if (bits.Length != 32) throw new ArgumentException("Must have 32 bits");
		_bits = bits;
	}

    public Int(int value)
    {
        _bits = new bool[32];
        for (int i = 0; i < 32; i++)
            _bits[i] = ((value >> i) & 1) == 1;
    }

    public Int(uint value)
    {
        _bits = new bool[32];
        for (int i = 0; i < 32; i++)
            _bits[i] = ((value >> i) & 1) == 1;
    }

    public Int Value => FromBits(_bits);
	public static int BitWidth => 32;

    public Int FromValue(bool value)
    {
        if (value)
        {
            // Return all ones (0xFFFFFFFF)
            var bits = new bool[32];
            for (int i = 0; i < 32; i++) bits[i] = true;
            return new Int(bits);
        }

        return new Int(0);
    }
    public Int FromBits(bool[] bits) => new Int(bits);
    public Bit GetBit(int index) => _bits[index];
	public Int SetBit(int index, bool value)
	{
		var newBits = (bool[])_bits.Clone();
		newBits[index] = value;
		return new Int(newBits);
	}

    public static implicit operator Int(bool value)
    {
		return new Int(value ? 1: 0);
    }

    public static implicit operator Int(int value)
	{
		var bits = new bool[32];
		for (int i = 0; i < 32; i++)
			bits[i] = ((value >> i) & 1) == 1;
		return new Int(bits);
	}

	public static implicit operator int(Int i)
	{
		int result = 0;
		for (int x = 0; x < 32; x++)
			if (i._bits[x]) result |= 1 << x;
		return result;
	}

	public static implicit operator Int(uint value)
	{
		var bits = new bool[32];
		for (int i = 0; i < 32; i++)
			bits[i] = ((value >> i) & 1) == 1;
		return new Int(bits);
	}

	public static implicit operator uint(Int i)
	{
		uint result = 0;
		for (int x = 0; x < 32; x++)
			if (i._bits[x]) result |= 1u << x;
		return result;
	}

	public static implicit operator Long(Int i)
	{
		var bits = new bool[64];
		for (int x = 0; x < 32; x++)
			bits[x] = i._bits[x];
		return new Long(bits);
	}

	public static explicit operator Int(Long l)
	{
		var bits = new bool[32];
		for (int x = 0; x < 32; x++)
			bits[x] = l.GetBit(x);
		return new Int(bits);
	}

	public static explicit operator Int(Short s)
	{
		var bits = new bool[32];
		for (int x = 0; x < 16; x++)
			bits[x] = s.GetBit(x);
		return new Int(bits);
	}

    public override string ToString()
	{
		uint result = 0;
		for (int i = 0; i < 32; i++)
			if (_bits[i]) result |= 1u << i;
		return result.ToString();
	}

	public string ToBinaryString()
		=> string.Concat(_bits.Reverse().Select(b => b ? "1" : "0"));

	public string ToHexString()
		=> ((uint)this).ToString("X8");

    public bool Equals(Int other)
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
