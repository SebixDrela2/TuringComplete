using Turing.Core.ComplexComponents;
using Turing.Core.Electricity;

internal class Program
{
    private static void Main(string[] args)
    {
        var shift = 1;
        var val = 2048;

        Int value = new LSR<Int>(val, shift);
    }
}