internal class Program
{
    private static void Main(string[] args)
    {
        var content = typeof(Program)
            .Assembly
            .GetManifestResourceStream("main.c");
    }
}