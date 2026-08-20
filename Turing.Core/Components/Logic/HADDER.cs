using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Core.Components.Logic;

/// <summary>
/// <br>HADDER commonly known as half adder is a first step towards achieving digital logic addition.</br>
/// <br>The difference between HADDER and ADDER is that HADDER does not take carry as input parameter</br>
/// <br>Hence the name Half adder.</br>
/// </summary>
/// <param name="inputA"></param>
/// <param name="inputB"></param>
public class HADDER(Bit inputA, Bit inputB) : TurComponent<(Bit Sum, Bit Carry)>
{
    protected override (Bit Sum, Bit Carry) ImplicitOperator()
    {
        Bit sum = new XOR<Bit>(inputA, inputB);
        Bit carry = new AND<Bit>(inputA, inputB);

        return (sum, carry);
    }
}

