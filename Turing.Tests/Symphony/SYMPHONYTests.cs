using Turing.Core.Computer.Symphony;

namespace Turing.Tests.Symphony;

[TestFixture]
internal partial class SYMPHONYTests
{
    private InstructionParser _parser;
    [SetUp]
    public void SetUp()
    {
        _parser = new();
    }

    [Test]
    public void Symphony_Capitalizes_Char()
    {
        var asm =
            """
            LOOP:
            in r6

            cmp r6, 32
            je IsSpace
            cmp r6, 97
            jge ToUpper
            cmp r6, 122
            jge ToUpper

            YieldReturn:
            out r6
            jmp SkipToSpace

            ToUpper:
            sub r6, r6, 32
            jmp YieldReturn

            IsSpace:
            out r6
            jmp LOOP

            SkipToSpace:
            in r6
            cmp r6, 32
            je IsSpace

            out r6
            jmp SkipToSpace      
            """;

        var instructions = _parser.Parse(asm);
        var cpu = RunSymphony(instructions, new Byte('a'));
    }
}
