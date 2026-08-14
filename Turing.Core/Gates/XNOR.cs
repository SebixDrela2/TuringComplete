using Turing.Core.Electricity;

namespace Turing.Core.Gates;

public class XNOR<T>(T inputA, T inputB) : IGate<T, T> where T : struct, IValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;

    public static implicit operator T(XNOR<T> gate)
    {
        T xorGate = new XOR<T>(gate._inputA, gate._inputB);
        T result = new NOT<T>(xorGate);
        
        return result;
    }
}
