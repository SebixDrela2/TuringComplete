using Turing.Core.Electricity;

namespace Turing.Core.Global;

public static class ValueExtensions
{
    extension(IValue value)
    {
        public Y Into<Y>() where Y : struct, IValue<Y>
        {
            var bits = value.ToBits();
            var arr = new bool[Y.BitWidth];

            bits.AsSpan(0, int.Min(bits.Length, Y.BitWidth))
                .CopyTo(arr);

            return Y.FromBits(arr);
        }
    }
}
