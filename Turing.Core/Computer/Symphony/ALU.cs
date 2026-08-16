using Turing.Core.Components.Arithmetic;
using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Computer.Symphony;

public class ALU(Byte op, Int a, Int b) : TurComponentValue<Int>
{
    protected override Int ImplicitOperator()
    {
        // Compute all results
        Int nandResult = new NAND<Int>(a, b);
        Int orResult = new OR<Int>(a, b);
        Int andResult = new AND<Int>(a, b);
        Int norResult = new NOR<Int>(a, b);
        Int addResult = Add(a, b);
        Int subResult = Subtract(a, b);

        // Extract opcode bits (LSB = bit5)
        Bit c0 = op.GetBit(0);
        Bit c1 = op.GetBit(1);
        Bit c2 = op.GetBit(2);

        Bit n0 = new NOT<Bit>(c0);
        Bit n1 = new NOT<Bit>(c1);
        Bit n2 = new NOT<Bit>(c2);

        // One‑hot decode for opcodes 0‑5 (6‑7 unused -> zero)
        Bit sel0 = new AND<Bit>(new AND<Bit>(n2, n1), n0); // 0: NAND
        Bit sel1 = new AND<Bit>(new AND<Bit>(n2, n1), c0); // 1: OR
        Bit sel2 = new AND<Bit>(new AND<Bit>(n2, c1), n0); // 2: AND
        Bit sel3 = new AND<Bit>(new AND<Bit>(n2, c1), c0); // 3: NOR
        Bit sel4 = new AND<Bit>(new AND<Bit>(c2, n1), n0); // 4: ADD
        Bit sel5 = new AND<Bit>(new AND<Bit>(c2, n1), c0); // 5: SUB

        Int selected0 = Select(sel0, nandResult);
        Int selected1 = Select(sel1, orResult);
        Int selected2 = Select(sel2, andResult);
        Int selected3 = Select(sel3, norResult);
        Int selected4 = Select(sel4, addResult);
        Int selected5 = Select(sel5, subResult);

        // OR all selected results
        Int or01 = new OR<Int>(selected0, selected1);
        Int or23 = new OR<Int>(selected2, selected3);
        Int or45 = new OR<Int>(selected4, selected5);
        Int or0123 = new OR<Int>(or01, or23);
        Int or012345 = new OR<Int>(or0123, or45);

        return or012345;
    }

    private static Int Add(Int a, Int b)
    {
        Bit zero = new Bit(0);
        (Int Sum, Bit Carry) = ((Int, Bit))new ADDER<Int>(a, b, zero);
        return Sum;
    }

    private static Int Subtract(Int a, Int b)
    {
        Int negB = new NEG<Int>(b);
        return Add(a, negB);
    }

    private static Int Select(Bit select, Int value)
    {
        Int mask = new Byte(select.Value ? 0xFF : 0x00);
        return new AND<Int>(value, mask);
    }
}
