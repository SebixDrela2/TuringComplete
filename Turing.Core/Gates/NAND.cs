using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Gates;

public class NAND<T>(T inputA, T inputB) : IGate<T, T> where T : struct, IValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;

    public static implicit operator T(NAND<T> gate) => new NOT<T>(new AND<T>(gate._inputA, gate._inputB));
}