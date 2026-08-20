using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Logic;

/// <summary>
/// <br>ADDER is the base component for all operational logic, it perfoms line bitwise addition for all supported data types.</br>
/// <br>It takes two inputs A and B as parameters as well as the optional carry.</br>
/// <br>Basic ADDER requires chaining different adders in order to support multi level bitness.</br>
/// <br>The more bits in data type the more work does the ADDER.</br>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="inputA"></param>
/// <param name="inputB"></param>
/// <param name="cin"></param>
public class ADDER<T>(T inputA, T inputB, Bit cin) : TurComponent<(T Sum, Bit Carry)> where T : struct, IValue<T>
{
    protected override (T Sum, Bit Carry) ImplicitOperator()
    {
        int bitWidth = T.BitWidth;
        var sumBits = new bool[bitWidth];
        Bit carry = new Bit((bool)cin.GetBit(0));

        for (int i = 0; i < bitWidth; i++)
        {
            var bitA = new Bit((bool)inputA.GetBit(i));
            var bitB = new Bit((bool)inputB.GetBit(i));

            var (sum1, carryOut) = ((Bit, Bit))new HADDER(bitA, bitB);
            var (finalSum, finalCarry) = ((Bit, Bit))new HADDER(sum1, carry);
            carry = new OR<Bit>(carryOut, finalCarry);

            sumBits[i] = finalSum.Value;
        }

        var sum = T.FromBits(sumBits);

        return (sum, carry);
    }
}