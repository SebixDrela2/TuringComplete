using Turing.Core.ComplexComponents;
using Turing.Core.Components.Arithmetic;
using Turing.Core.Components.Logic;
using Turing.Core.Electricity;
using Turing.Core.Gates;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Computer.Symphony;

public class ALU(Byte op, Int a, Int b) : TurComponentValue<Int>
{
    protected override Int ImplicitOperator()
    {
        // Compute all results
        Int nandResult = new NAND<Int>(a, b);
        Int orResult = new OR<Int>(a, b);
        Int andResult = new AND<Int>(a, b);
        Int norResult = new NOR<Int>(a, b);
        Int addResult = Add(a, b);
        Int subResult = Subtract(a, b);
        Int xorResult = new XOR<Int>(a, b);
        Int lslResult = new LSL<Int>(a, b);
        Int lsrResult = new LSR<Int>(a, b);
        Int asrResult = new ASR<Int>(a, b);
        Int cmpResult = new CMP_FLAGS(a, b);

        Bit c0 = op.GetBit(0);
        Bit c1 = op.GetBit(1);
        Bit c2 = op.GetBit(2);
        Bit c3 = op.GetBit(3);

        Bit n0 = new NOT<Bit>(c0);
        Bit n1 = new NOT<Bit>(c1);
        Bit n2 = new NOT<Bit>(c2);
        Bit n3 = new NOT<Bit>(c3);

        Bit sel0 = new AND<Bit>(n3, new AND<Bit>(new AND<Bit>(n2, n1), n0));  // 0: NAND
        Bit sel1 = new AND<Bit>(n3, new AND<Bit>(new AND<Bit>(n2, n1), c0));  // 1: OR
        Bit sel2 = new AND<Bit>(n3, new AND<Bit>(new AND<Bit>(n2, c1), n0));  // 2: AND
        Bit sel3 = new AND<Bit>(n3, new AND<Bit>(new AND<Bit>(n2, c1), c0));  // 3: NOR
        Bit sel4 = new AND<Bit>(n3, new AND<Bit>(new AND<Bit>(c2, n1), n0));  // 4: ADD
        Bit sel5 = new AND<Bit>(n3, new AND<Bit>(new AND<Bit>(c2, n1), c0));  // 5: SUB
        Bit sel6 = new AND<Bit>(n3, new AND<Bit>(new AND<Bit>(c2, c1), n0));  // 6: XOR
        Bit sel7 = new AND<Bit>(n3, new AND<Bit>(new AND<Bit>(c2, c1), c0));  // 7: LSL
        Bit sel8 = new AND<Bit>(c3, new AND<Bit>(new AND<Bit>(n2, n1), n0));  // 8: LSR
        Bit sel9 = new AND<Bit>(c3, new AND<Bit>(new AND<Bit>(n2, n1), c0));  // 9: ASR
        Bit sel10 = new AND<Bit>(c3, new AND<Bit>(new AND<Bit>(n2, c1), n0)); // 10: CMP

        Int selected0 = new SW<Int>(sel0, nandResult);
        Int selected1 = new SW<Int>(sel1, orResult);
        Int selected2 = new SW<Int>(sel2, andResult);
        Int selected3 = new SW<Int>(sel3, norResult);
        Int selected4 = new SW<Int>(sel4, addResult);
        Int selected5 = new SW<Int>(sel5, subResult);
        Int selected6 = new SW<Int>(sel6, xorResult);
        Int selected7 = new SW<Int>(sel7, lslResult);
        Int selected8 = new SW<Int>(sel8, lsrResult);
        Int selected9 = new SW<Int>(sel9, asrResult);
        Int selected10 = new SW<Int>(sel10, cmpResult);

        Int selected = new OR<Int>(selected0, selected1);
        selected = new OR<Int>(selected, selected2);
        selected = new OR<Int>(selected, selected3);
        selected = new OR<Int>(selected, selected4);
        selected = new OR<Int>(selected, selected5);
        selected = new OR<Int>(selected, selected6);
        selected = new OR<Int>(selected, selected7);
        selected = new OR<Int>(selected, selected8);
        selected = new OR<Int>(selected, selected9);
        selected = new OR<Int>(selected, selected10);

        return selected;
    }

    private static Int Add(Int a, Int b)
    {
        Bit zero = new Bit(0);
        (Int Sum, Bit Carry) = ((Int, Bit))new ADDER<Int>(a, b, zero);

        return Sum;
    }

    private static Int Subtract(Int a, Int b)
    {
        Int negB = new NEG<Int>(b);

        return Add(a, negB);
    }
}
