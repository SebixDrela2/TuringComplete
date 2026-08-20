using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Arithmetic;

/// <summary>
/// <br>NEG commonly known as Negator component serves purpose of arithemtically negating bits.</br>
/// <br>Example: 5 becomes -5 instead of -6 for NOT, this is why we need to add 1</br>
/// <br>This is basically Two's complement, which negator operates in.</br>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="value"></param>
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