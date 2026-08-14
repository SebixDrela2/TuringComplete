using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

namespace Turing.Core.Computers;

public abstract class Processor
{
    protected CLOCK Clock { get; }
    public Processor()
    {
        Clock = new CLOCK();
    }
    
    public void EVal()
    {
        Step();
        Clock.Tick();

        Step();
        Clock.Tick();
    }

    protected abstract void Step();
}
