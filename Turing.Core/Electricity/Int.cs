using System.Diagnostics;

namespace Turing.Core.Electricity;

[DebuggerDisplay($"{{Number,nq}}")]
public record struct Int : IByteValue<Int>
{
	private bool[] _bits;
	public bool[] Bits => _bits ??= new bool[32];

	public static Int Zero => 0;
	public static Int One => 1;

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

    public Int Value => FromBits(Bits);

	public int Number => (int)Value;

	public bool[] ToBits() => Bits;
	public static int BitWidth => 32;

    public static Int FromValue(bool value)
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
    public static Int FromBits(bool[] bits) => new Int(bits);
    public Bit GetBit(int index) => Bits[index];
    public void SetBit(int index, bool value)
    {
        Bits[index] = value;
    }
    public static implicit operator Int(Bit bit) => new Byte(bit);

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
			if (i.Bits[x]) result |= 1 << x;
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
			if (i.Bits[x]) result |= 1u << x;
		return result;
	}

	public static implicit operator Long(Int i)
	{
		var bits = new bool[64];
		for (int x = 0; x < 32; x++)
			bits[x] = i.Bits[x];
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
			if (Bits[i]) result |= 1u << i;
		return result.ToString();
	}

	public string ToBinaryString()
		=> string.Concat(Bits.Reverse().Select(b => b ? "1" : "0"));

	public string ToHexString()
		=> ((uint)this).ToString("X8");

    public Bit LastBit() => Bits[BitWidth - 1];
    public bool Equals(Int other)
    {
        if (Bits.Length != other.Bits.Length) return false;
        for (int i = 0; i < Bits.Length; i++)
            if (Bits[i] != other.Bits[i]) return false;
        return true;
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        foreach (var bit in Bits)
            hashCode.Add(bit);
        return hashCode.ToHashCode();
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
