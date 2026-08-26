using Turing.Core.Gates;

namespace Turing.Core.Computer.Symphony;

/// <summary>
/// <br>SYMPHONY conditional component.</br>
/// </summary>
/// <param name="flags"></param>
/// <param name="cond"></param>
internal class COND(Byte flags, Byte cond) : TurComponent<Bit>
{
    protected override Bit ImplicitOperator()
    {
        Bit f0 = flags.GetBit(0);
        Bit f1 = flags.GetBit(1);
        Bit f2 = flags.GetBit(2);

        Bit c0 = cond.GetBit(0);
        Bit c1 = cond.GetBit(1);
        Bit c2 = cond.GetBit(2);
        Bit cNEG = cond.GetBit(3);

        Bit a0 = new AND<Bit>(f0, c0);
        Bit a1 = new AND<Bit>(f1, c1);
        Bit a2 = new AND<Bit>(f2, c2);

        Bit or = new OR<Bit>(a0, a1);
        or = new OR<Bit>(or, a2);

        Bit result = new XOR<Bit>(or, cNEG);

        return result;
    }
}
