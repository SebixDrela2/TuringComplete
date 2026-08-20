using Turing.Core.Electricity;

namespace Turing.Core.Gates;

/// <summary>
/// <br>XNOR gate returns current if ALL inputs are OFF or ALL inputs are ON.</br>
/// <br></br>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="inputA"></param>
/// <param name="inputB"></param>
public class XNOR<T>(T inputA, T inputB) : TurComponentValue<T>, IGate<T, T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        T xorGate = new XOR<T>(inputA, inputB);
        T result = new NOT<T>(xorGate);
        
        return result;
    }
}
