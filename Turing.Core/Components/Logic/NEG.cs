using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Arithmetic;

/// <summary>
/// Two's complement negation: -value = ~value + 1
/// </summary>
public class NEG<T> where T : struct, IValue<T>
{
    private readonly T _result;

    public T Result => _result;

    public NEG(T value)
    {
        T notVal = new NOT<T>(value);
        CONST<T> oneConst = new CONST<T>(1);
        T one = (T)oneConst;
        T zero = value.FromValue(false);
        (T Sum, Bit Carry) = ((T, Bit)) new ADDER<T>(notVal, one, zero);
        _result = Sum;
    }

    public static implicit operator T(NEG<T> neg)
    {
        return neg._result;
    }
}