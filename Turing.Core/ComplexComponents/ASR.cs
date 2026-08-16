using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.ComplexComponents;

public class ASR<T>(T input, Int bitShift) where T : struct, IByteValue<T>, IValue<T>
{
    private readonly T _input = input;
    private readonly Int _bitShift = bitShift;

    public static implicit operator T(ASR<T> asr)
    {
        T result = 0;
        T notInput = new NOT<T>(asr._input);

        var bit0 = asr._bitShift.GetBit(0);
        var bit1 = asr._bitShift.GetBit(1);
        var bit2 = asr._bitShift.GetBit(2);
        var bit3 = asr._bitShift.GetBit(3);
        var bit4 = asr._bitShift.GetBit(4);
        var signBit = asr._input.GetBit(T.BitWidth - 1);
        var pinsToChange = T.BitWidth - 1;
        var indexable = T.BitWidth - 1;
        var shiftAmount = 1;
        var currDecoderIndex = 0;

        Int decoded = new BIT_DECODER_FIVE(bit4, bit3, bit2, bit1, bit0, T.Zero);
        result = new SW<T>(decoded.GetBit(currDecoderIndex), asr._input);

        while (pinsToChange > 0)
        {
            T swapped = 0;
            var idx = 1;

            for (var i = indexable; i > shiftAmount; i--) 
            {
                swapped.SetBit(indexable - idx - shiftAmount, asr._input.GetBit(indexable - idx));
                idx++;
            }

            var signSwapCount = shiftAmount + 1;
            var index = 1;

            while(signSwapCount > 0)
            {
                swapped.SetBit(T.BitWidth - index, signBit);
                signSwapCount--;
                index++;
            }

            result = new OR<T>(new SW<T>(decoded.GetBit(shiftAmount), swapped), result);
            shiftAmount++;
            pinsToChange--;
        }

        return result;
    }
}
