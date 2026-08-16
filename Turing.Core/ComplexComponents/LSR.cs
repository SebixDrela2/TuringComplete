using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.ComplexComponents;


public class LSR<T>(T input, Int bitShift) : TurComponentValue<T> where T : struct, IByteValue<T>, IValue<T>
{
    protected override T ImplicitOperator()
    {
        T result = 0;
        T notInput = new NOT<T>(input);

        for (var i = 0; i < T.BitWidth; i++)
        {
            var bit0 = bitShift.GetBit(0);
            var bit1 = bitShift.GetBit(1);
            var bit2 = bitShift.GetBit(2);
            var bit3 = bitShift.GetBit(3);
            var bit4 = bitShift.GetBit(4);

            Bit disableBit = notInput.GetBit(i);
            Bit enableBit = (T)new NOT<T>(disableBit);
            Int decoded = new BIT_DECODER_FIVE(bit4, bit3, bit2, bit1, bit0, disableBit);
            Int swapped = 0;

            for (var j = i; j >= 0; j--)
            {
                var first = i - j;
                var last = j;

                swapped.SetBit(first, decoded.GetBit(last));
            }

            result = new OR<T>(new SW<T>(enableBit, (int)swapped), (int)result);
        }

        return result;
    }
}
