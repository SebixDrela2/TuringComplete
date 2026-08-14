using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Logic;

public class ADDER<T>(T inputA, T inputB, T cin) where T : struct, IValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;
    private readonly T _cin = cin;

    public static implicit operator (T Sum, Bit Carry)(ADDER<T> adder)
    {
        int bitWidth = T.BitWidth;
        var sumBits = new bool[bitWidth];
        Bit carry = new Bit((bool)adder._cin.GetBit(0));

        for (int i = 0; i < bitWidth; i++)
        {
            var bitA = new Bit((bool)adder._inputA.GetBit(i));
            var bitB = new Bit((bool)adder._inputB.GetBit(i));

            var (sum1, carryOut) = ((Bit, Bit))new HADDER<Bit>(bitA, bitB);
            var (finalSum, finalCarry) = ((Bit, Bit))new HADDER<Bit>(sum1, carry);
            var or = new OR<Bit>(carryOut, finalCarry);
            carry = (Bit)or;

            sumBits[i] = finalSum.Value;
        }

        var sum = adder._inputA.FromBits(sumBits);

        return (sum, carry.Value);
    }
}