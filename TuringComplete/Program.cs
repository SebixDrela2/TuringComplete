using Turing.Core.ComplexComponents;
using Turing.Core.Electricity;
using Byte = Turing.Core.Electricity.Byte;

internal class Program
{
    private static void Main(string[] args)
    {
        var shift = 1;
        var val = 16;

        Byte value = new LSR<Byte>(val, shift);
    }
}