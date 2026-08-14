using Turing.Core.ComplexComponents;
using Turing.Core.Electricity;
using Byte = Turing.Core.Electricity.Byte;

internal class Program
{
    private static void Main(string[] args)
    {
        var shift = 3;
        var val = 1;

        Byte value = new LSR<Byte>(shift, val);
    }
}