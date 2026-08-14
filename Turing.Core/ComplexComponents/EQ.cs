using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.ComplexComponents;

public class EQ<T>(T inputA, T inputB) where T : struct, IByteValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;

    public static implicit operator Bit(EQ<T> eq)
    {
        Bit result = 1;
        
        T xor = new XOR<T>(eq._inputA, eq._inputB);

        for (var i = 0; i < T.BitWidth; i = i + 2)
        {
            result = new AND<Bit>(new NOR<Bit>(xor.GetBit(i), xor.GetBit(i + 1)), result);
        } 

        return result;
    }
}
