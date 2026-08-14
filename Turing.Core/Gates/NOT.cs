using Turing.Core.Electricity;

namespace Turing.Core.Gates;

public class NOT<T>(T input) : IGate<T, T> where T : struct, IValue<T>
{
    private readonly T _input = input;

    public static implicit operator T(NOT<T> gate)
    {
        T nand = new NAND<T>(gate._input, gate._input);

        return nand;
    }
}
