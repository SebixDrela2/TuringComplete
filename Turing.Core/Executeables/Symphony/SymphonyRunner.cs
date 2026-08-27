using Turing.Core.Computer.Symphony;

namespace Turing.Core.Executeables.Symphony;

public class SymphonyRunner
{
    private readonly InstructionParser _parser = new();

    public SYMPHONY RunSymphony(string asm, params IEnumerable<Byte> inputs)
    {
        var instructions = _parser.Parse(asm);

        return RunSymphony(instructions, inputs);
    }

    public SYMPHONY RunSymphony(Byte[] instructions, params IEnumerable<Byte> inputs)
    {
        var cpu = new SYMPHONY(instructions);

        var en = inputs.GetEnumerator();
        bool hasNext = en.MoveNext();
        cpu.Input = hasNext ? en.Current : default;


        while (true)
        {
            cpu.EVal();

            if (cpu.InputPin)
            {
                if (!hasNext)
                {
                    return cpu;
                }

                hasNext = en.MoveNext();
                cpu.Input = hasNext ? en.Current : default;
            }

            if (cpu.OutputPin)
            {
                return cpu;
            }

            if (cpu.OffPin)
            {
                return cpu;
            }
        }
    }
}
