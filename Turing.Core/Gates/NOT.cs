using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Gates;

/// <summary>
/// <br>NOT gate inverts the current from the input and returns such in the output</br>
/// <br>Unlike NPN transistor (NSW) it does not require gate param</br>
/// <br>Complexity: 1</br>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="input"></param>
public class NOT<T>(T input) : TurComponentValue<T>, IGate<T, T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        var result = T.Zero;

        for (int i = 0; i < T.BitWidth; i++)
        {
            Bit bit = new NSW<Bit>(input.GetBit(i), Bit.One);
            result.SetBit(i, bit);
        }

        return result;
    }
}