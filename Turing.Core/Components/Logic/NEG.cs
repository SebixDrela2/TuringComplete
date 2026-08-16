using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Arithmetic;

/// <summary>
/// Two's complement negation: -value = ~value + 1
/// </summary>
public class NEG<T>(T value) : TurComponentValue<T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator()
    {
        T notVal = new NOT<T>(value);
        CONST<T> oneConst = new CONST<T>(1);
        T one = (T)oneConst;
        (T Sum, Bit Carry) = ((T, Bit)) new ADDER<T>(notVal, one, T.Zero);

        return Sum;
    }
}