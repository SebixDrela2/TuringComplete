global using Turing.Core.Global;
global using static Turing.Core.Global.GlobalScope;
global using Bit = Turing.Core.Electricity.Bit;
global using Byte = Turing.Core.Electricity.Byte;
global using Long = Turing.Core.Electricity.Long;
global using Short = Turing.Core.Electricity.Short;

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
public static partial class GlobalScope;
