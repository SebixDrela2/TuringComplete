using Turing.Core.Electricity;

namespace Turing.Core.Gates;

/// <summary>
/// XOR gate returns current ONLY if one of inputs is ON.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="inputA"></param>
/// <param name="inputB"></param>
public class XOR<T>(T inputA, T inputB) : TurComponentValue<T>, IGate<T, T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        T orGate = new OR<T>(inputA, inputB);
        T nandGate = new NAND<T>(inputA, inputB);
        T result = new AND<T>(orGate, nandGate);

        return result;
    }
}
