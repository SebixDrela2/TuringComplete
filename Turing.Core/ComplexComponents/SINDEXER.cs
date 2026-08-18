using Turing.Core.Components.Arithmetic;
using Turing.Core.Components.Logic;
using Turing.Core.Electricity;

namespace Turing.Core.ComplexComponents;

public class SINDEXER<T>(T input, Int shift) : TurComponentValue<T>
    where T : struct, IByteValue<T>
{
    protected override T ImplicitOperator()
    {
        if (shift < 0)
        {
            return (T)(input << -shift);
        }

        return (T)(input >> shift);

        var negBit = shift.LastBit();

        return new MUX<T>(
            new LSR<T>(input, shift), 
            new LSL<T>(input, 
                new NEG<Int>(shift)
            ), 
            negBit
        );
    }
}
