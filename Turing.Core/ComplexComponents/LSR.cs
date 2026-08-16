using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.ComplexComponents;


public class LSR<T>(T input, Int bitShift) : TurComponent<T> where T : struct, IByteValue<T>, IValue<T>
{
    private readonly T _input = input;
    private readonly Int _bitShift = bitShift;

    protected override T ImplicitOperator()
    {
        T result = 0;
        T notInput = new NOT<T>(_input);

        for (var i = 0; i < T.BitWidth; i++)
        {
            var bit0 = _bitShift.GetBit(0);
            var bit1 = _bitShift.GetBit(1);
            var bit2 = _bitShift.GetBit(2);
            var bit3 = _bitShift.GetBit(3);
            var bit4 = _bitShift.GetBit(4);

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
