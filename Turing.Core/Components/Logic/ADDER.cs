using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Logic;

public class ADDER<T>(T inputA, T inputB, T cin) where T : struct, IBitValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;
    private readonly T _cin = cin;

    public static implicit operator (T Sum, T Carry)(ADDER<T> adder)
    {
        int bitWidth = adder._inputA.BitWidth;
        var sumBits = new bool[bitWidth];
        Bit carry = new Bit(adder._cin.GetBit(0));

        for (int i = 0; i < bitWidth; i++)
        {
            var bitA = new Bit(adder._inputA.GetBit(i));
            var bitB = new Bit(adder._inputB.GetBit(i));

            var (sum1, carryOut) = ((Bit, Bit))new HADDER<Bit>(bitA, bitB);
            var (finalSum, finalCarry) = ((Bit, Bit))new HADDER<Bit>(sum1, carry);
            var or = new OR<Bit>(carryOut, finalCarry);
            carry = (Bit)or;

            sumBits[i] = finalSum.Value;
        }

        var sum = adder._inputA.FromBits(sumBits);

        var carryBits = new bool[bitWidth];
        carryBits[0] = carry.Value;
        var carryResult = adder._inputA.FromBits(carryBits);

        return (sum, carryResult);
    }
}