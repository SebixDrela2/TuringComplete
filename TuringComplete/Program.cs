using Turing.Core.ComplexComponents;
using Turing.Core.Electricity;
using Byte = Turing.Core.Electricity.Byte;

internal class Program
{
    private static void Main(string[] args)
    {
        var shift = 6;
        var val = -124;

        Byte value = new ASR<Byte>(val, shift);
    }
}