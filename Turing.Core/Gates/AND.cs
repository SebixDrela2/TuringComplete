using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Gates;

/// <summary>
/// <br>AND is just a PN Transistor, a switch</br>
/// <br>It flows current only if both inputs are ON</br>
/// <br>AND compared to SW can support multiple data types on the gate parameter.</br>
/// <br>Complexity: 1</br>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="inputA"></param>
/// <param name="inputB"></param>
public class AND<T>(T inputA, T inputB) : TurComponentValue<T>, IGate<T, T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        var result = T.Zero;

        for (int i = 0; i < T.BitWidth; i++)
        {
            Bit bit = new SW<Bit>(
              inputA.GetBit(i),
              inputB.GetBit(i)
            );
            result.SetBit(i, bit);
        }

        return result;
    }
}