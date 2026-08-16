using Turing.Core.Electricity;

namespace Turing.Core.Gates;

public class OR<T>(T inputA, T inputB) : TurComponentValue<T>, IGate<T, T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        T notA = new NOT<T>(inputA);
        T notB = new NOT<T>(inputB);
        T result = new NAND<T>(notA, notB);

        return result;
    }
}
