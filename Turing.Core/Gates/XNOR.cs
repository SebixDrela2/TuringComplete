using Turing.Core.Electricity;

namespace Turing.Core.Gates;

public class XNOR<T>(T inputA, T inputB) : TurComponentValue<T>, IGate<T, T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        T xorGate = new XOR<T>(inputA, inputB);
        T result = new NOT<T>(xorGate);
        
        return result;
    }
}
