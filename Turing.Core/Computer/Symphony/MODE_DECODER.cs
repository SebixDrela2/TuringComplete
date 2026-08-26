using Turing.Core.Gates;

namespace Turing.Core.Computer.Symphony;

/// <summary>
/// <br>SYMPHONY mode decoder, 2 bit decoder which takes BYTE as an input, returns IO,ALU,JUMP,RAM mode selectively.</br>
/// </summary>
/// <param name="mode"></param>
internal class MODE_DECODER(Byte mode) : TurComponent<(Bit IO, Bit ALU, Bit JUMP, Bit RAM)>
{
    protected override (Bit IO, Bit ALU, Bit JUMP, Bit RAM) ImplicitOperator()
    {
        Bit c0 = mode.GetBit(0);
        Bit c1 = mode.GetBit(1);

        Bit n0 = new NOT<Bit>(c0);
        Bit n1 = new NOT<Bit>(c1);

        Bit y0 = new AND<Bit>(n1, n0);
        Bit y1 = new AND<Bit>(n1, c0);
        Bit y2 = new AND<Bit>(c1, n0);
        Bit y3 = new AND<Bit>(c1, c0);

        return (y0, y1, y2, y3);
    }
}