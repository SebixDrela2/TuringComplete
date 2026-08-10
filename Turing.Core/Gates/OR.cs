using Turing.Core.Electricity;

namespace Turing.Core.Gates;

public class OR<T>(T inputA, T inputB) : IGate<T, T> where T : struct, IBitValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;

    public static implicit operator T(OR<T> gate)
    {
        T notA = new NOT<T>(gate._inputA);
        T notB = new NOT<T>(gate._inputB);
        T result = new NAND<T>(notA, notB);

        return result;
    }
}
