using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.ComplexComponents;

public class LSL<T>(T input, Int bitShift) : TurComponentValue<T> where T : struct, IByteValue<T>, IValue<T>
{
    protected override T ImplicitOperator()
    { 
        T result = 0;
        T notInput = new NOT<T>(input);

        var bit0 = bitShift.GetBit(0);
        var bit1 = bitShift.GetBit(1);
        var bit2 = bitShift.GetBit(2);
        var bit3 = bitShift.GetBit(3);
        var bit4 = bitShift.GetBit(4);
        var signBit = input.GetBit(T.BitWidth - 1);
        var pinsToChange = T.BitWidth - 1;
        var indexable = T.BitWidth - 1;
        var shiftAmount = 1;

        Int decoded = new BIT_DECODER_FIVE(bit4, bit3, bit2, bit1, bit0, T.Zero);
        result = new SW<T>(decoded.GetBit(0), input);

        while (pinsToChange > 0)
        {
            T swapped = 0;
            var idx = 0;

            for (var i = shiftAmount; i <= indexable; i++)
            {
                swapped.SetBit(shiftAmount + idx, input.GetBit(idx));
                idx++;
            }

            result = new OR<T>(new SW<T>(decoded.GetBit(shiftAmount), swapped), result);
            shiftAmount++;
            pinsToChange--;
        }

        return result;
    }
}
