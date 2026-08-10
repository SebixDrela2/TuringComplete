using Turing.Core.Components.Arithmetic;
using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Overture;

/// <summary>
/// Arithmetic Logic Unit with 6 operations:
/// Opcode 0: NAND
/// Opcode 1: OR
/// Opcode 2: AND
/// Opcode 3: NOR
/// Opcode 4: ADD (a + b)
/// Opcode 5: SUB (a - b)
/// The opcode is taken from bits 5,6,7 of the opcode byte.
/// </summary>
public class ALU
{
    private readonly Byte _result;

    public Byte Result => _result;

    public ALU(Byte op, Byte a, Byte b)
    {
        // Compute all results
        Byte nandResult = new NAND<Byte>(a, b);
        Byte orResult = new OR<Byte>(a, b);
        Byte andResult = new AND<Byte>(a, b);
        Byte norResult = new NOR<Byte>(a, b);
        Byte addResult = Add(a, b);
        Byte subResult = Subtract(a, b);

        // Extract opcode bits (LSB = bit5)
        Bit c0 = op.GetBit(5);
        Bit c1 = op.GetBit(6);
        Bit c2 = op.GetBit(7);

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

        Byte selected0 = Select(sel0, nandResult);
        Byte selected1 = Select(sel1, orResult);
        Byte selected2 = Select(sel2, andResult);
        Byte selected3 = Select(sel3, norResult);
        Byte selected4 = Select(sel4, addResult);
        Byte selected5 = Select(sel5, subResult);

        // OR all selected results
        Byte or01 = new OR<Byte>(selected0, selected1);
        Byte or23 = new OR<Byte>(selected2, selected3);
        Byte or45 = new OR<Byte>(selected4, selected5);
        Byte or0123 = new OR<Byte>(or01, or23);
        Byte or012345 = new OR<Byte>(or0123, or45);
        _result = or012345;
    }

    private static Byte Add(Byte a, Byte b)
    {
        Byte zero = new Byte(0);
        (Byte Sum, Bit Carry) = ((Byte, Bit)) new ADDER<Byte>(a, b, zero);
        return Sum;
    }

    private static Byte Subtract(Byte a, Byte b)
    {
        // a - b = a + (-b)
        NEG<Byte> neg = new NEG<Byte>(b);
        Byte negB = (Byte)neg;
        return Add(a, negB);
    }

    private static Byte Select(Bit select, Byte value)
    {
        Byte mask = new Byte(select.Value ? 0xFF : 0x00);
        return new AND<Byte>(value, mask);
    }

    public static implicit operator Byte(ALU alu)
    {
        return alu._result;
    }
}