using Turing.Core.ComplexComponents;
using Turing.Core.Electricity;
using Byte = Turing.Core.Electricity.Byte;

internal class Program
{
    private static void Main(string[] args)
    {
        var shift = 7;
        var val = 1;

        Byte value = new LSL<Byte>(val, shift);
    }
}