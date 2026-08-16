using Turing.Core.ComplexComponents;
using Turing.Core.Electricity;

namespace Turing.Core.Computer.Symphony;

internal class CMP_FLAGS(Int A, Int B) : TurComponentValue<Int>
{
    protected override Int ImplicitOperator()
    {
        Bit eq = new EQ<Int>(A, B);
        Bit low = new LOW<Int>(A, B);
        Bit less = new LESS<Int>(A, B);

        Int result = new Int();
        result.SetBit(0, eq);
        result.SetBit(1, low);
        result.SetBit(2, less);

        return result;
    }
}
