using Turing.Core.Electricity;

namespace Turing.Core.Gates;

public class AND<T>(T inputA, T inputB) : IGate<T, T> where T : struct, IValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;

    public static implicit operator T(AND<T> gate)
    {
        T nand = new NAND<T>(gate._inputA, gate._inputB);
        T not = new NOT<T>(nand);

        return not;
    }
}
