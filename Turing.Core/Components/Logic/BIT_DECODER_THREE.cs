using Turing.Core.Gates;

namespace Turing.Core.Components.Logic;

public class BIT_DECODER_THREE(Bit inputA, Bit inputB, Bit inputC, Bit disable)
{
    private readonly Bit _inputA = inputA;
    private readonly Bit _inputB = inputB;
    private readonly Bit _inputC = inputC;
    private readonly Bit _disable = disable;

    public static implicit operator Byte(BIT_DECODER_THREE decoder)
    {
        var notA = new NOT<Bit>(decoder._inputA);
        var notB = new NOT<Bit>(decoder._inputB);
        var notC = new NOT<Bit>(decoder._inputC);

        var notA_Result = (Bit)notA;
        var notB_Result = (Bit)notB;
        var notC_Result = (Bit)notC;

        var notDisable = new NOT<Bit>(decoder._disable);
        var enable = (Bit)notDisable;
        var enableT = Bit.FromValue(enable.Value);

        Bit y0 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), notC_Result),
            enableT
        );

        Bit y1 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), decoder._inputC),
            enableT
        );

        Bit y2 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(notA_Result, decoder._inputB), notC_Result),
            enableT
        );

        Bit y3 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(notA_Result, decoder._inputB), decoder._inputC),
            enableT
        );

        Bit y4 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(decoder._inputA, notB_Result), notC_Result),
            enableT
        );

        Bit y5 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(decoder._inputA, notB_Result), decoder._inputC),
            enableT
        );

        Bit y6 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(decoder._inputA, decoder._inputB), notC_Result),
            enableT
        );

        Bit y7 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(decoder._inputA, decoder._inputB), decoder._inputC),
            enableT
        );

        return new Byte([y0, y1, y2, y3, y4, y5, y6, y7]);
    }
}