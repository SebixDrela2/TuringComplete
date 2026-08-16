using Turing.Core.Gates;

namespace Turing.Core.Components.Logic;

public class BIT_DECODER_THREE(Bit inputA, Bit inputB, Bit inputC, Bit disable) : TurComponentValue<Byte>
{
    protected override Byte ImplicitOperator()
    {
        var notA = new NOT<Bit>(inputA);
        var notB = new NOT<Bit>(inputB);
        var notC = new NOT<Bit>(inputC);

        var notA_Result = (Bit)notA;
        var notB_Result = (Bit)notB;
        var notC_Result = (Bit)notC;

        var notDisable = new NOT<Bit>(disable);
        var enable = (Bit)notDisable;
        var enableT = Bit.FromValue(enable.Value);

        Bit y0 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), notC_Result),
            enableT
        );

        Bit y1 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), inputC),
            enableT
        );

        Bit y2 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(notA_Result, inputB), notC_Result),
            enableT
        );

        Bit y3 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(notA_Result, inputB), inputC),
            enableT
        );

        Bit y4 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(inputA, notB_Result), notC_Result),
            enableT
        );

        Bit y5 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(inputA, notB_Result), inputC),
            enableT
        );

        Bit y6 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(inputA, inputB), notC_Result),
            enableT
        );

        Bit y7 = new AND<Bit>(
            new AND<Bit>(new AND<Bit>(inputA, inputB), inputC),
            enableT
        );

        return new Byte([y0, y1, y2, y3, y4, y5, y6, y7]);
    }
}