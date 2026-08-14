using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.ComplexComponents;

public class LSR<T>(T input, Byte bitShift) where T : struct, IByteValue<T>, IValue<T>
{
    private readonly T _input = input;
    private readonly Byte _bitShift = bitShift;

    public static implicit operator T(LSR<T> lsr)
    {
        T result = 0;
        T notInput = new NOT<T>(lsr._input);

        for (var i = 0; i < T.BitWidth; i++)
        {
            var bit0 = lsr._bitShift.GetBit(0);
            var bit1 = lsr._bitShift.GetBit(1);
            var bit2 = lsr._bitShift.GetBit(2);

            Bit disableBit = notInput.GetBit(i);
            Bit enableBit = (T)new NOT<T>(disableBit);
            Byte decoded = new BIT_DECODER_THREE(bit2, bit1, bit0, disableBit);

            Byte swapped = new Byte(0);

            int loopAmount = (int)(T)new SW<T>(enableBit, i + 1);

            for (var j = loopAmount - 1; j >= 0; j--)
            {
                var first = i - j;
                var last = j;

                swapped.SetBit(first, decoded.GetBit(last));
            }

            result = new OR<T>((int)swapped, (int)result);
        }

        return result;
    }
}
