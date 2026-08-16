using Turing.Core.Electricity;

namespace Turing.Core.Gates;

public class NAND<T>(T inputA, T inputB) : TurComponentValue<T>, IGate<T, T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator() => new NOT<T>(new AND<T>(inputA, inputB));
}