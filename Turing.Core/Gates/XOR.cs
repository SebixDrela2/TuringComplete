using Turing.Core.Electricity;

namespace Turing.Core.Gates;

public class XOR<T>(T inputA, T inputB) : IGate<T, T> where T : struct, IValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;

    public static implicit operator T(XOR<T> gate)
    {
        T orGate = new OR<T>(gate._inputA, gate._inputB);
        T nandGate = new NAND<T>(gate._inputA, gate._inputB);
        T result = new AND<T>(orGate, nandGate);

        return result;
    }
}
