using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.ComplexComponents;

public class EQ<T>(T inputA, T inputB) : TurComponentValue<T> where T : struct, IByteValue<T>
{
    protected override T ImplicitOperator()
    {
        Bit result = 1;

        T xor = new XOR<T>(inputA, inputB);

        for (var i = 0; i < T.BitWidth; i = i + 2)
        {
            result = new AND<Bit>(new NOR<Bit>(xor.GetBit(i), xor.GetBit(i + 1)), result);
        }

        return result;
    }
}
