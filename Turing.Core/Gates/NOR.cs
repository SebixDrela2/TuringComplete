using Turing.Core.Electricity;

namespace Turing.Core.Gates;

/// <summary>
/// <br>NOR gate only outputs current when both inputs are OFF.</br>
/// <br>It is usually most commonly made out of NOTted output of OR</br>
/// <br>Complexity: 2</br>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="inputA"></param>
/// <param name="inputB"></param>
public class NOR<T>(T inputA, T inputB) : TurComponentValue<T>, IGate<T, T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        T orGate = new OR<T>(inputA, inputB);
        T result = new NOT<T>(orGate);

        return result;
    }
}