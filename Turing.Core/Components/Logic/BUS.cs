using Turing.Core.Electricity;

namespace Turing.Core.Components.Logic;

public class BUS<T>(T inputA, T inputB, Bit sel0, Bit sel1) where T : struct, IBitValue<T>
{
    private readonly T _inputA = inputA;
    private readonly T _inputB = inputB;
    private readonly Bit _sel0 = sel0; 
    private readonly Bit _sel1 = sel1; 

    public static implicit operator (T OutputA, T OutputB)(BUS<T> bus)
    {
        T muxA = new MUX<T>(bus._inputA, bus._inputB, bus._sel0);
        T muxB = new MUX<T>(bus._inputA, bus._inputB, bus._sel1);

        return (muxA, muxB);
    }
}
