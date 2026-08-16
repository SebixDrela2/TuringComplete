using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Logic;

public class ADDER<T>(T inputA, T inputB, Bit cin) : TurComponent<(T Sum, Bit Carry)> where T : struct, IValue<T>
{
    private readonly T inputA = inputA;
    private readonly T inputB = inputB;
    private readonly Bit cin = cin;

    protected override (T Sum, Bit Carry) ImplicitOperator()
    {
        int bitWidth = T.BitWidth;
        var sumBits = new bool[bitWidth];
        Bit carry = new Bit((bool)cin.GetBit(0));

        for (int i = 0; i < bitWidth; i++)
        {
            var bitA = new Bit((bool)inputA.GetBit(i));
            var bitB = new Bit((bool)inputB.GetBit(i));

            var (sum1, carryOut) = ((Bit, Bit))new HADDER<Bit>(bitA, bitB);
            var (finalSum, finalCarry) = ((Bit, Bit))new HADDER<Bit>(sum1, carry);
            var or = new OR<Bit>(carryOut, finalCarry);
            carry = (Bit)or;

            sumBits[i] = finalSum.Value;
        }

        var sum = T.FromBits(sumBits);

        return (sum, carry);
    }
}