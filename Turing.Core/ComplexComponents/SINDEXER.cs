using Turing.Core.Components.Arithmetic;
using Turing.Core.Components.Logic;
using Turing.Core.Electricity;

namespace Turing.Core.ComplexComponents;

public class SINDEXER<T>(T input, Int shift) where T : struct, IByteValue<T>
{
    private readonly T _input = input;
    private readonly Int _shift = shift;

    public static implicit operator T(SINDEXER<T> indexer)
    {
        var negBit = indexer._shift.LastBit();

        return new MUX<T>(
            new LSR<T>(indexer._input, indexer._shift), 
            new LSL<T>(indexer._input, 
                new NEG<Int>(indexer._shift)
            ), 
            negBit
        );
    }
}
