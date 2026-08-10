using Turing.Core.Electricity;

namespace Turing.Core.Gates;

public class NOR<T>(T inputA, T inputB) : IGate<T, T> where T : struct, IBitValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;

    public static implicit operator T(NOR<T> gate)
    {
        T orGate = new OR<T>(gate._inputA, gate._inputB);
        T result = new NOT<T>(orGate);

        return result;
    }
}