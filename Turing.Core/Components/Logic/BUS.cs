using Turing.Core.Electricity;

namespace Turing.Core.Components.Logic;

public class BUS<T>(T inputA, T inputB, Bit sel0, Bit sel1) : TurComponent<(T A, T B)> where T : struct, IValue<T>
{
    protected override (T A, T B) ImplicitOperator()
    {
        T muxA = new MUX<T>(inputA, inputB, sel0);
        T muxB = new MUX<T>(inputA, inputB, sel1);

        return (muxA, muxB);
    }
}
