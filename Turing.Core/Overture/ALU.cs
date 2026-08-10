using Turing.Core.Gates;

namespace Turing.Core.Overture;

/// <summary>
/// Arithmetic Logic Unit with 4 operations:
/// Opcode 0: NAND
/// Opcode 1: OR
/// Opcode 2: AND
/// Opcode 3: NOR
/// The opcode is taken from bits 5,6,7 of the opcode byte.
/// </summary>
public class ALU
{
    private readonly Byte _result;

    public Byte Result => _result;

    public ALU(Byte op, Byte a, Byte b)
    {
        Byte nandResult = new NAND<Byte>(a, b);
        Byte orResult = new OR<Byte>(a, b);
        Byte andResult = new AND<Byte>(a, b);
        Byte norResult = new NOR<Byte>(a, b);

        Bit c0 = op.GetBit(5);
        Bit c1 = op.GetBit(6);
        Bit c2 = op.GetBit(7);

        Bit n0 = new NOT<Bit>(c0);
        Bit n1 = new NOT<Bit>(c1);
        Bit n2 = new NOT<Bit>(c2);

        Bit sel0 = new AND<Bit>(new AND<Bit>(n2, n1), n0); 
        Bit sel1 = new AND<Bit>(new AND<Bit>(n2, n1), c0); 
        Bit sel2 = new AND<Bit>(new AND<Bit>(n2, c1), n0); 
        Bit sel3 = new AND<Bit>(new AND<Bit>(n2, c1), c0); 

        Byte selected0 = Select(sel0, nandResult);
        Byte selected1 = Select(sel1, orResult);
        Byte selected2 = Select(sel2, andResult);
        Byte selected3 = Select(sel3, norResult);

        Byte or01 = new OR<Byte>(selected0, selected1);
        Byte or23 = new OR<Byte>(selected2, selected3);
        _result = new OR<Byte>(or01, or23);
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