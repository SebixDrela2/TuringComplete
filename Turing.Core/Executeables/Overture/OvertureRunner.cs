using Turing.Core.Overture;

namespace Turing.Core.Executeables.Overture;

public class OvertureRunner
{
    public OVERTURE RunOverture(Byte[] instructions, params IEnumerable<Byte> inputs)
    {
        var cpu = new OVERTURE(instructions);

        var en = inputs.GetEnumerator();
        cpu.Input = en.MoveNext() ? en.Current : default;

        while (true)
        {
            cpu.EVal();

            if (cpu.InputPin)
            {
                cpu.Input = en.MoveNext() ? en.Current : default;
            }

            if (cpu.OffPin || cpu.OutputPin)
            {
                return cpu;
            }
        }
    }
}
