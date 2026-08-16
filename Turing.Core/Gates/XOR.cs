using Turing.Core.Electricity;

namespace Turing.Core.Gates;

public class XOR<T>(T inputA, T inputB) : TurComponentValue<T>, IGate<T, T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        T orGate = new OR<T>(inputA, inputB);
        T nandGate = new NAND<T>(inputA, inputB);
        T result = new AND<T>(orGate, nandGate);

        return result;
    }
}
