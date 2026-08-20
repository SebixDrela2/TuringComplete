using Turing.Core.Electricity;

namespace Turing.Core.Components.Logic;

/// <summary>
/// <br>BUS has four inputs A and B as well as input selector and output selector.</br>
/// <br>Input selector decides which input is taken and output selector decides where that input goes to.</br>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="inputA"></param>
/// <param name="inputB"></param>
/// <param name="inputSel"></param>
/// <param name="outputSel"></param>
public class BUS<T>(T inputA, T inputB, Bit inputSel, Bit outputSel) : TurComponent<(T A, T B)> where T : struct, IValue<T>
{
    protected override (T A, T B) ImplicitOperator()
    {
        T muxA = new MUX<T>(inputA, inputB, inputSel);
        T muxB = new MUX<T>(inputA, inputB, outputSel);

        return (muxA, muxB);
    }
}
