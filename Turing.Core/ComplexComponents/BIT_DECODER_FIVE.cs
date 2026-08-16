using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Logic;

public class BIT_DECODER_FIVE(Bit inputA, Bit inputB, Bit inputC, Bit inputD, Bit inputE, Bit disable) : TurComponentValue<Int>
{
    protected override Int ImplicitOperator()
    {
        var notA = new NOT<Bit>(inputA);
        var notB = new NOT<Bit>(inputB);
        var notC = new NOT<Bit>(inputC);
        var notD = new NOT<Bit>(inputD);
        var notE = new NOT<Bit>(inputE);

        var notA_Result = (Bit)notA;
        var notB_Result = (Bit)notB;
        var notC_Result = (Bit)notC;
        var notD_Result = (Bit)notD;
        var notE_Result = (Bit)notE;

        var notDisable = new NOT<Bit>(disable);
        var enable = (Bit)notDisable;
        var enableT = Bit.FromValue(enable.Value);

        Bit y0 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), notC_Result),
                    notD_Result
                ),
                notE_Result
            ),
            enableT
        );

        Bit y1 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), notC_Result),
                    notD_Result
                ),
                inputE
            ),
            enableT
        );

        Bit y2 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), notC_Result),
                    inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y3 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), notC_Result),
                    inputD
                ),
                inputE
            ),
            enableT
        );

        Bit y4 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), inputC),
                    notD_Result
                ),
                notE_Result
            ),
            enableT
        );

        Bit y5 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), inputC),
                    notD_Result
                ),
                inputE
            ),
            enableT
        );

        Bit y6 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), inputC),
                    inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y7 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), inputC),
                    inputD
                ),
                inputE
            ),
            enableT
        );

        Bit y8 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, inputB), notC_Result),
                    notD_Result
                ),
                notE_Result
            ),
            enableT
        );

        Bit y9 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, inputB), notC_Result),
                    notD_Result
                ),
                inputE
            ),
            enableT
        );

        Bit y10 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, inputB), notC_Result),
                    inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y11 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, inputB), notC_Result),
                    inputD
                ),
                inputE
            ),
            enableT
        );

        Bit y12 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, inputB), inputC),
                    notD_Result
                ),
                notE_Result
            ),
            enableT
        );

        Bit y13 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, inputB), inputC),
                    notD_Result
                ),
                inputE
            ),
            enableT
        );

        Bit y14 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, inputB), inputC),
                    inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y15 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, inputB), inputC),
                    inputD
                ),
                inputE
            ),
            enableT
        );

        Bit y16 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, notB_Result), notC_Result),
                    notD_Result
                ),
                notE_Result
            ),
            enableT
        );

        Bit y17 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, notB_Result), notC_Result),
                    notD_Result
                ),
                inputE
            ),
            enableT
        );

        Bit y18 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, notB_Result), notC_Result),
                    inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y19 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, notB_Result), notC_Result),
                    inputD
                ),
                inputE
            ),
            enableT
        );

        Bit y20 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, notB_Result), inputC),
                    notD_Result
                ),
                notE_Result
            ),
            enableT
        );

        Bit y21 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, notB_Result), inputC),
                    notD_Result
                ),
                inputE
            ),
            enableT
        );

        Bit y22 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, notB_Result), inputC),
                    inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y23 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, notB_Result), inputC),
                    inputD
                ),
                inputE
            ),
            enableT
        );

        Bit y24 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, inputB), notC_Result),
                    notD_Result
                ),
                notE_Result
            ),
            enableT
        );

        Bit y25 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, inputB), notC_Result),
                    notD_Result
                ),
                inputE
            ),
            enableT
        );

        Bit y26 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, inputB), notC_Result),
                    inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y27 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, inputB), notC_Result),
                    inputD
                ),
                inputE
            ),
            enableT
        );

        Bit y28 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, inputB), inputC),
                    notD_Result
                ),
                notE_Result
            ),
            enableT
        );

        Bit y29 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, inputB), inputC),
                    notD_Result
                ),
                inputE
            ),
            enableT
        );

        Bit y30 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, inputB), inputC),
                    inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y31 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(inputA, inputB), inputC),
                    inputD
                ),
                inputE
            ),
            enableT
        );

        return new Int([y0, y1, y2, y3, y4, y5, y6, y7, y8, y9, y10, y11, y12, y13, y14, y15, y16, y17, y18, y19, y20, y21, y22, y23, y24, y25, y26, y27, y28, y29, y30, y31]);
    }
}