using Turing.Core.Electricity;
using Turing.Core.Gates;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Overture;

/// <summary>
/// Condition Component
/// Condition Byte uses bits 5, 6, 7 as a 3-bit condition code.
/// 
/// Cond Logic:
/// 000 -> Never
/// 001 -> Always
/// 010 -> if value == 0
/// 011 -> if value != 0
/// 100 -> if value < 0
/// 101 -> if value >= 0
/// 110 -> if value <= 0
/// 111 -> if value > 0
/// </summary>
public class COND(Byte value, Byte condition) : TurComponentValue<Bit>
{
    protected override Bit ImplicitOperator()
    {
        // Extract the three condition bits
        Bit c0 = condition.GetBit(0); // LSB of the condition code
        Bit c1 = condition.GetBit(1);
        Bit c2 = condition.GetBit(2); // MSB

        // NOT versions
        Bit n0 = new NOT<Bit>(c0);
        Bit n1 = new NOT<Bit>(c1);
        Bit n2 = new NOT<Bit>(c2);

        // Decode the 3-bit code into 8 one‑hot selects (code = (c2 << 2) | (c1 << 1) | c0)
        //  000 -> Never
        //  001 -> Always
        //  010 -> == 0
        //  011 -> != 0
        //  100 -> < 0
        //  101 -> >= 0
        //  110 -> <= 0
        //  111 -> > 0
        Bit selNever = new AND<Bit>(new AND<Bit>(n2, n1), n0); // 000
        Bit selAlways = new AND<Bit>(new AND<Bit>(n2, n1), c0); // 001
        Bit selEqZero = new AND<Bit>(new AND<Bit>(n2, c1), n0); // 010
        Bit selNeZero = new AND<Bit>(new AND<Bit>(n2, c1), c0); // 011
        Bit selLtZero = new AND<Bit>(new AND<Bit>(c2, n1), n0); // 100
        Bit selGeZero = new AND<Bit>(new AND<Bit>(c2, n1), c0); // 101
        Bit selLeZero = new AND<Bit>(new AND<Bit>(c2, c1), n0); // 110
        Bit selGtZero = new AND<Bit>(new AND<Bit>(c2, c1), c0); // 111

        // Compute value comparisons
        Bit isZero = IsZero(value);
        Bit isNegative = new Bit((bool)value.GetBit(Byte.BitWidth - 1)); // MSB is sign bit

        Bit isPositive = new AND<Bit>(new NOT<Bit>(isZero), new NOT<Bit>(isNegative));
        Bit isNonNegative = new NOT<Bit>(isNegative);
        Bit isNonPositive = new NOT<Bit>(isPositive);

        // Result for each condition
        Bit resNever = new Bit(false);
        Bit resAlways = new Bit(true);
        Bit resEqZero = isZero;
        Bit resNeZero = new NOT<Bit>(isZero);
        Bit resLtZero = isNegative;
        Bit resGeZero = isNonNegative;
        Bit resLeZero = isNonPositive;
        Bit resGtZero = isPositive;

        // Select the correct result using AND with the one‑hot selects
        Bit r0 = new SW<Bit>(selNever, resNever);
        Bit r1 = new SW<Bit>(selAlways, resAlways);
        Bit r2 = new SW<Bit>(selEqZero, resEqZero);
        Bit r3 = new SW<Bit>(selNeZero, resNeZero);
        Bit r4 = new SW<Bit>(selLtZero, resLtZero);
        Bit r5 = new SW<Bit>(selGeZero, resGeZero);
        Bit r6 = new SW<Bit>(selLeZero, resLeZero);
        Bit r7 = new SW<Bit>(selGtZero, resGtZero);

        // OR all selected results together
        Bit or12 = new OR<Bit>(r0, r1);
        Bit or34 = new OR<Bit>(r2, r3);
        Bit or56 = new OR<Bit>(r4, r5);
        Bit or78 = new OR<Bit>(r6, r7);
        Bit or1234 = new OR<Bit>(or12, or34);
        Bit or5678 = new OR<Bit>(or56, or78);

        return new OR<Bit>(or1234, or5678);
    }

    private static Bit IsZero(Byte value)
    {
        Bit result = new Bit(true);
        for (int i = 0; i < Byte.BitWidth; i++)
        {
            Bit bit = new Bit((bool)value.GetBit(i));
            result = new AND<Bit>(result, new NOT<Bit>(bit));
        }
        return result;
    }
}