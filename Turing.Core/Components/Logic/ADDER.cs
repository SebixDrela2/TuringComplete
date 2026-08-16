using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Logic;

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