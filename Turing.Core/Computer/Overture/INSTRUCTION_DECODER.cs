using Turing.Core.Gates;

namespace Turing.Core.Overture;

internal class INSTRUCTION_DECODER(Byte input) : TurComponent<(Bit Immediate, Bit ALU, Bit Move, Bit Condition)>
{
    protected override (Bit Immediate, Bit ALU, Bit Move, Bit Condition) ImplicitOperator()
    {
        var bits = input.Bits;
        Bit bit7 = bits[6];
        Bit bit8 = bits[7];

        Bit immiediate = new NOR<Bit>(bit7, bit8);
        Bit alu = new AND<Bit>(bit7, new NOT<Bit>(bit8));
        Bit move = new AND<Bit>(new NOT<Bit>(bit7), bit8);
        Bit cond = new AND<Bit>(bit7, bit8);

        return (immiediate, alu, move, cond);
    }
}
