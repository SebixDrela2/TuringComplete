using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Logic;

public class BIT_DECODER_FIVE(Bit inputA, Bit inputB, Bit inputC, Bit inputD, Bit inputE, Bit disable)
{
    private readonly Bit _inputA = inputA;
    private readonly Bit _inputB = inputB;
    private readonly Bit _inputC = inputC;
    private readonly Bit _inputD = inputD;
    private readonly Bit _inputE = inputE;
    private readonly Bit _disable = disable;

    public static implicit operator Int(BIT_DECODER_FIVE decoder)
    {
        var notA = new NOT<Bit>(decoder._inputA);
        var notB = new NOT<Bit>(decoder._inputB);
        var notC = new NOT<Bit>(decoder._inputC);
        var notD = new NOT<Bit>(decoder._inputD);
        var notE = new NOT<Bit>(decoder._inputE);

        var notA_Result = (Bit)notA;
        var notB_Result = (Bit)notB;
        var notC_Result = (Bit)notC;
        var notD_Result = (Bit)notD;
        var notE_Result = (Bit)notE;

        var notDisable = new NOT<Bit>(decoder._disable);
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
                decoder._inputE
            ),
            enableT
        );

        Bit y2 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), notC_Result),
                    decoder._inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y3 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), notC_Result),
                    decoder._inputD
                ),
                decoder._inputE
            ),
            enableT
        );

        Bit y4 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), decoder._inputC),
                    notD_Result
                ),
                notE_Result
            ),
            enableT
        );

        Bit y5 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), decoder._inputC),
                    notD_Result
                ),
                decoder._inputE
            ),
            enableT
        );

        Bit y6 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), decoder._inputC),
                    decoder._inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y7 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, notB_Result), decoder._inputC),
                    decoder._inputD
                ),
                decoder._inputE
            ),
            enableT
        );

        Bit y8 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, decoder._inputB), notC_Result),
                    notD_Result
                ),
                notE_Result
            ),
            enableT
        );

        Bit y9 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, decoder._inputB), notC_Result),
                    notD_Result
                ),
                decoder._inputE
            ),
            enableT
        );

        Bit y10 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, decoder._inputB), notC_Result),
                    decoder._inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y11 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, decoder._inputB), notC_Result),
                    decoder._inputD
                ),
                decoder._inputE
            ),
            enableT
        );

        Bit y12 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, decoder._inputB), decoder._inputC),
                    notD_Result
                ),
                notE_Result
            ),
            enableT
        );

        Bit y13 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, decoder._inputB), decoder._inputC),
                    notD_Result
                ),
                decoder._inputE
            ),
            enableT
        );

        Bit y14 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, decoder._inputB), decoder._inputC),
                    decoder._inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y15 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(notA_Result, decoder._inputB), decoder._inputC),
                    decoder._inputD
                ),
                decoder._inputE
            ),
            enableT
        );

        Bit y16 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, notB_Result), notC_Result),
                    notD_Result
                ),
                notE_Result
            ),
            enableT
        );

        Bit y17 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, notB_Result), notC_Result),
                    notD_Result
                ),
                decoder._inputE
            ),
            enableT
        );

        Bit y18 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, notB_Result), notC_Result),
                    decoder._inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y19 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, notB_Result), notC_Result),
                    decoder._inputD
                ),
                decoder._inputE
            ),
            enableT
        );

        Bit y20 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, notB_Result), decoder._inputC),
                    notD_Result
                ),
                notE_Result
            ),
            enableT
        );

        Bit y21 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, notB_Result), decoder._inputC),
                    notD_Result
                ),
                decoder._inputE
            ),
            enableT
        );

        Bit y22 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, notB_Result), decoder._inputC),
                    decoder._inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y23 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, notB_Result), decoder._inputC),
                    decoder._inputD
                ),
                decoder._inputE
            ),
            enableT
        );

        Bit y24 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, decoder._inputB), notC_Result),
                    notD_Result
                ),
                notE_Result
            ),
            enableT
        );

        Bit y25 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, decoder._inputB), notC_Result),
                    notD_Result
                ),
                decoder._inputE
            ),
            enableT
        );

        Bit y26 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, decoder._inputB), notC_Result),
                    decoder._inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y27 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, decoder._inputB), notC_Result),
                    decoder._inputD
                ),
                decoder._inputE
            ),
            enableT
        );

        Bit y28 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, decoder._inputB), decoder._inputC),
                    notD_Result
                ),
                notE_Result
            ),
            enableT
        );

        Bit y29 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, decoder._inputB), decoder._inputC),
                    notD_Result
                ),
                decoder._inputE
            ),
            enableT
        );

        Bit y30 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, decoder._inputB), decoder._inputC),
                    decoder._inputD
                ),
                notE_Result
            ),
            enableT
        );

        Bit y31 = new AND<Bit>(
            new AND<Bit>(
                new AND<Bit>(
                    new AND<Bit>(new AND<Bit>(decoder._inputA, decoder._inputB), decoder._inputC),
                    decoder._inputD
                ),
                decoder._inputE
            ),
            enableT
        );

        return new Int([y0, y1, y2, y3, y4, y5, y6, y7, y8, y9, y10, y11, y12, y13, y14, y15, y16, y17, y18, y19, y20, y21, y22, y23, y24, y25, y26, y27, y28, y29, y30, y31]);
    }
}