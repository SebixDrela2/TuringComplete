using Turing.Core.Components.Arithmetic;
using Turing.Core.Components.Logic;
using Turing.Core.Gates;

namespace Turing.Core.Overture;

/// <summary>
/// <br>This is an Arithemtic Logic unit which supports 6 basic operations with 3 inputs</br>
/// <br>Inputs are as follows Byte opcode, Byte A, Byte B</br>
/// <br>The opcode is taken from bits 5,6,7 of the opcode byte.</br>
/// <br>Opcode 0: NAND</br>
/// <br>Opcode 1: OR</br>
/// <br>Opcode 2: AND</br>
/// <br>Opcode 3: NOR</br>
/// <br>Opcode 4: ADD</br>
/// <br>Opcode 5: SUB</br>
/// </summary>
public class ALU(Byte op, Byte a, Byte b) : TurComponentValue<Byte>
{
    protected override Byte ImplicitOperator()
    {       
        Byte nandResult = new NAND<Byte>(a, b);
        Byte orResult = new OR<Byte>(a, b);
        Byte andResult = new AND<Byte>(a, b);
        Byte norResult = new NOR<Byte>(a, b);
        Byte addResult = Add(a, b);
        Byte subResult = Subtract(a, b);
    
        Bit c0 = op.GetBit(0);
        Bit c1 = op.GetBit(1);
        Bit c2 = op.GetBit(2);

        Bit n0 = new NOT<Bit>(c0);
        Bit n1 = new NOT<Bit>(c1);
        Bit n2 = new NOT<Bit>(c2);
  
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

        Byte or01 = new OR<Byte>(selected0, selected1);
        Byte or23 = new OR<Byte>(selected2, selected3);
        Byte or45 = new OR<Byte>(selected4, selected5);
        Byte or0123 = new OR<Byte>(or01, or23);
        Byte or012345 = new OR<Byte>(or0123, or45);

        return or012345;
    }

    private static Byte Add(Byte a, Byte b)
    {
        Bit zero = new Bit(0);
        (Byte Sum, Bit Carry) = ((Byte, Bit)) new ADDER<Byte>(a, b, zero);

        return Sum;
    }

    private static Byte Subtract(Byte a, Byte b)
    {
        NEG<Byte> neg = new NEG<Byte>(b);
        Byte negB = (Byte)neg;

        return Add(a, negB);
    }

    private static Byte Select(Bit select, Byte value)
    {
        Byte mask = new Byte(select.Value ? 0xFF : 0x00);

        return new AND<Byte>(value, mask);
    }
}