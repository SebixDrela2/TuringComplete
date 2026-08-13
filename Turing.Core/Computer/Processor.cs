using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Computers;

public abstract class Processor<T>
    where T : struct, IBitValue<T>
{
    protected CLOCK Clock { get; } = new CLOCK();

    public void EVal(T instruction, T inputData)
    {
        Step(instruction, inputData);
        Clock.Tick();

        Step(instruction, inputData);
        Clock.Tick();
    }

    protected abstract void Step(T instruction, T inputData);
}
