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
            const RES   = r1
            const ARG_1 = r1
            const ARG_2 = r2
          
            in ARG_1 
            in ARG_2 

            call power 
            out RES 

            multiply:             
                push r3

                const LHS = r1 
                const RHS = r2 
                const ACC = r3 

                mov ACC, 0

                jmp mul_condition
                mul_start:
                sub RHS, RHS, 1
                add ACC, ACC, LHS
                mul_condition:
                cmp RHS, 0
                jne mul_start

                mov RES, ACC
             
                pop r3

                ret

            power:
                push r3
                push r4

                const BASE = r3
                const REM_POW = r4 

                mov BASE, ARG_1
                sub REM_POW, ARG_2, 1

                
                pow_start:
                sub REM_POW, REM_POW, 1

                mov ARG_2, BASE

                call multiply

                pow_condition:
                cmp REM_POW, 0
                jne pow_start
             
                pop r4
                pop r3

                ret           
            """;

        var asm2 = 
            """
            add r3, r3, r1
            """;

        var instructions = _parser.Parse(asm);
        var cpu = RunSymphony(instructions, new Byte(0x2), new Byte(0x3));
    }
}
