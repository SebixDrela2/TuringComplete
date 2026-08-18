using Turing.Core.Electricity;

namespace Turing.Core.Global;

public static class ValueExtensions
{
    extension(IValue value)
    {
        public Y Into<Y>() where Y: struct, IValue<Y>
        {
            return value.Into<Y>(Y.BitWidth);
        }

        public Y Into<Y>(int bitWidth) where Y : struct, IValue<Y>
        {
            var bits = value.ToBits();
            var arr = new bool[Y.BitWidth];

            bits.AsSpan(0, int.Min(bits.Length, bitWidth))
                .CopyTo(arr);

            return Y.FromBits(arr);
        }
    }
}
