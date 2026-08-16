using Turing.Core.Electricity;

namespace Turing.Core.Gates;

public class NOR<T>(T inputA, T inputB) : TurComponentValue<T>, IGate<T, T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        T orGate = new OR<T>(inputA, inputB);
        T result = new NOT<T>(orGate);

        return result;
    }
}