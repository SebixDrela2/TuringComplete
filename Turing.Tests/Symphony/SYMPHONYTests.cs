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

    private SYMPHONY Run(string asm, params Byte[] inputs)
    {
        var instructions = _parser.Parse(asm);
        return RunSymphony(instructions, inputs);
    }

    [Test]
    public void Test_Nop()
    {
        var asm = @"
                nop
                out 42
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(42)));
    }

    [Test]
    public void Test_In_Register()
    {
        var asm = @"
                in r1
                out r1
            ";
        var cpu = Run(asm, 171); // 0xAB in decimal
        Assert.That(cpu.Output, Is.EqualTo(new Int(171)));
    }

    [Test]
    public void Test_Out_Register()
    {
        var asm = @"
                mov r1, 127
                out r1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(127)));
    }

    [Test]
    public void Test_Out_Immediate()
    {
        var asm = "out 85"; // 0x55 in decimal
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(85)));
    }

    [Test]
    public void Test_Counter()
    {
        var asm = @"
                counter r1
                out r1
            ";
        var cpu = Run(asm);
        Assert.Pass();
    }

    [Test]
    public void Test_Nand_Reg()
    {
        var asm = @"
                mov r1, 15
                mov r2, 51
                nand r3, r2, r1
                out r3
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(4294967292))); // 0xFC
    }

    [Test]
    public void Test_Or_Reg()
    {
        var asm = @"
                mov r1, 15
                mov r2, 51
                or r3, r2, r1
                out r3
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(63))); // 0x3F
    }

    [Test]
    public void Test_And_Reg()
    {
        var asm = @"
                mov r1, 15
                mov r2, 51
                and r3, r1, r2
                out r3
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(3))); // 0x03
    }

    [Test]
    public void Test_Nor_Reg()
    {
        var asm = @"
                mov r1, 15
                mov r2, 51
                nor r4, r2, r1
                out r4
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(4294967232))); // 0xC0
    }

    [Test]
    public void Test_Add_Reg()
    {
        var asm = @"
                mov r1, 16
                mov r2, 32
                add r3, r1, r2
                out r3
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(48))); // 0x30
    }

    [Test]
    public void Test_Sub_Reg()
    {
        var asm = @"
                mov r2, 80
                mov r1, 32
                sub r3, r2, r1
                out r3
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(48))); // 0x30
    }

    [Test]
    public void Test_Xor_Reg()
    {
        var asm = @"
                mov r1, 15
                mov r2, 51
                xor r3, r1, r2
                out r3
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(60))); // 0x3C
    }

    [Test]
    public void Test_Lsl_Reg()
    {
        var asm = @"
                mov r1, 1
                mov r2, 2
                lsl r3, r1, r2
                out r3
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(4))); // 0x04
    }

    [Test]
    public void Test_Lsr_Reg()
    {
        var asm = @"
                mov r1, 8
                mov r2, 2
                lsr r4, r1, r2
                out r4
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(2))); // 0x02
    }

    [Test]
    public void Test_Asr_Reg()
    {
        var asm = @"
                mov r1, 128
                mov r2, 1
                asr r3, r1, r2
                out r3
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(64))); // 0xC0
    }

    [Test]
    public void Test_Nand_Imm()
    {
        var asm = @"
                mov r2, 15
                nand r1, r2, 51
                out r1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(4294967292)));
    }

    [Test]
    public void Test_Or_Imm()
    {
        var asm = @"
                mov r2, 15
                or r1, r2, 51
                out r1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(63))); // 0x3F
    }

    [Test]
    public void Test_And_Imm()
    {
        var asm = @"
                mov r1, 15
                and r2, r1, 51
                out r2
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(3))); // 0x03
    }

    [Test]
    public void Test_Nor_Imm()
    {
        var asm = @"
                mov r1, 15
                nor r2, r1, 51
                out r2
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(4294967232))); // 0xC0
    }

    [Test]
    public void Test_Add_Imm()
    {
        var asm = @"
                mov r2, 16
                add r1, r2, 32
                out r1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(48))); // 0x30
    }

    [Test]
    public void Test_Sub_Imm()
    {
        var asm = @"
                mov r1, 80
                sub r2, r1, 32
                out r2
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(48))); // 0x30
    }

    [Test]
    public void Test_Xor_Imm()
    {
        var asm = @"
                mov r1, 15
                xor r2, r1, 51
                out r2
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(60))); // 0x3C
    }

    [Test]
    public void Test_Lsl_Imm()
    {
        var asm = @"
                mov r1, 1
                lsl r2, r1, 2
                out r2
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(4))); // 0x04
    }

    [Test]
    public void Test_Lsr_Imm()
    {
        var asm = @"
                mov r1, 8
                lsr r2, r1, 2
                out r2
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(2))); // 0x02
    }

    [Test]
    public void Test_Asr_Imm()
    {
        var asm = @"
                mov r2, 128
                asr r1, r2, 1
                out r1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(64))); // 0xC0
    }

    [Test]
    public void Test_Cmp_Reg()
    {
        var asm = @"
                mov r1, 5
                mov r2, 5
                cmp r1, r2
                je equal
                out 0
                halt:
                jmp halt
                equal:
                out 1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(1)));
    }

    [Test]
    public void Test_Cmp_Imm()
    {
        var asm = @"
                mov r1, 5
                cmp r1, 5
                je equal
                out 0
                halt:
                jmp halt
                equal:
                out 1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(1)));
    }

    [Test]
    public void Test_Jmp_Register()
    {
        var asm = @"
                mov r1, target
                jmp r1
                out 0
                target:
                out 1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(1)));
    }

    [Test]
    public void Test_Jmp_Label()
    {
        var asm = @"
                jmp target
                out 0
                target:
                out 1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(1)));
    }

    [Test]
    public void Test_Je()
    {
        var asm = @"
                mov r1, 5
                cmp r1, 5
                je equal
                out 0
                halt:
                jmp halt
                equal:
                out 1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(1)));
    }

    [Test]
    public void Test_Jne()
    {
        var asm = @"
                mov r1, 5
                cmp r1, 6
                jne not_equal
                out 0
                halt:
                jmp halt
                not_equal:
                out 1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(1)));
    }

    [Test]
    public void Test_Jb()
    {
        var asm = @"
                mov r1, 5
                cmp r1, 6
                jb less
                out 0
                halt:
                jmp halt
                less:
                out 1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(1)));
    }

    [Test]
    public void Test_Jae()
    {
        var asm = @"
                mov r1, 6
                cmp r1, 5
                jae ge
                out 0
                halt:
                jmp halt
                ge:
                out 1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(1)));
    }

    [Test]
    public void Test_Jbe()
    {
        var asm = @"
                mov r1, 5
                cmp r1, 6
                jbe le
                out 0
                halt:
                jmp halt
                le:
                out 1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(1)));
    }

    [Test]
    public void Test_Ja()
    {
        var asm = @"
                mov r1, 7
                cmp r1, 6
                ja greater
                out 0
                halt:
                jmp halt
                greater:
                out 1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(1)));
    }

    [Test]
    public void Test_Jl()
    {
        var asm = @"
                mov r1, 5
                cmp r1, 6
                jl less
                out 0
                halt:
                jmp halt
                less:
                out 1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(1)));
    }

    [Test]
    public void Test_Jge()
    {
        var asm = @"
                mov r1, 6
                cmp r1, 5
                jge ge
                out 0
                halt:
                jmp halt
                ge:
                out 1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(1)));
    }

    [Test]
    public void Test_Jle()
    {
        var asm = @"
                mov r1, 5
                cmp r1, 6
                jle le
                out 0
                halt:
                jmp halt
                le:
                out 1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(1)));
    }

    [Test]
    public void Test_Jg()
    {
        var asm = @"
                mov r1, 7
                cmp r1, 6
                jg greater
                out 0
                halt:
                jmp halt
                greater:
                out 1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(1)));
    }

    [Test]
    public void Test_Load8_Reg()
    {
        var asm = @"
                mov r4, 170
                store_8 [r1], r4
                mov r2, r1
                load_8 r3, [r2]
                out r3
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(170))); // 0xAA
    }

    [Test]
    public void Test_Load8_Imm()
    {
        var asm = @"
                mov r1, 69
                store_8 [100], r1
                load_8 r4, [100]
                out r4
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(69))); // 0xBB
    }

    [Test]
    public void Test_Load16_Reg()
    {
        var asm = @"
                mov r4, 52445
                store_16 [r1], r4
                mov r2, r1
                load_16 r3, [r2]
                out r3
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(52445))); // 0xCCDD
    }

    [Test]
    public void Test_Load16_Imm()
    {
        var asm = @"
                mov r3, 69
                store_16 [20], r3
                load_16 r1, [20]
                out r1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(69))); // 0xEEFF
    }

    [Test]
    public void Test_Load32_Reg()
    {
        var asm = @"
                mov r4, 6900
                store_32 [r1], r4
                mov r2, r1
                load_32 r3, [r2]
                out r3
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(6900))); // 0x12345678
    }

    [Test]
    public void Test_Load32_Imm()
    {
        var asm = @"
                mov r5, 10000
                store_32 [30], r5
                load_32 r1, [30]
                out r1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(10000))); // 0x9ABCDEF0
    }

    [Test]
    public void Test_Store8_Reg()
    {
        var asm = @"
                mov r1, 66
                store_8 [r2], r1
                load_8 r3, [r2]
                out r3
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(66))); // 0x42
    }

    [Test]
    public void Test_Store8_Imm()
    {
        var asm = @"
                mov r1, 67
                store_8 [40], r1
                load_8 r2, [40]
                out r2
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(67))); // 0x43
    }

    [Test]
    public void Test_Store16_Reg()
    {
        var asm = @"
                mov r1, 4386
                store_16 [r2], r1
                load_16 r3, [r2]
                out r3
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(4386))); // 0x1122
    }

    [Test]
    public void Test_Store16_Imm()
    {
        var asm = @"
                mov r1, 13124
                store_16 [50], r1
                load_16 r2, [50]
                out r2
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(13124))); // 0x3344
    }

    [Test]
    public void Test_Store32_Reg()
    {
        var asm = @"
                mov r1, 13124
                store_32 [r2], r1
                load_32 r3, [r2]
                out r3
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(13124))); // 0xAABBCCDD
    }

    [Test]
    public void Test_Store32_Imm()
    {
        var asm = @"
                mov r1, 60000
                store_32 [60000], r1
                load_32 r2, [60000]
                out r2
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(60000))); // 0x11223344
    }

    [Test]
    public void Test_Mov_Reg()
    {
        var asm = @"
                mov r1, 85
                mov r2, r1
                out r2
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(85))); // 0x55
    }

    [Test]
    public void Test_Mov_Imm()
    {
        var asm = "mov r1, 170\nout r1";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(170))); // 0xAA
    }

    [Test]
    public void Test_Neg_Reg()
    {
        var asm = @"
                mov r1, 5
                neg r2, r1
                out r2
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(4294967291))); // 0xFB
    }

    [Test]
    public void Test_Neg_Imm()
    {
        var asm = @"
                neg r1, 5
                out r1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(4294967291))); // 0xFB
    }

    [Test]
    public void Test_Not_Reg()
    {
        var asm = @"
                mov r1, 15
                not r2, r1
                out r2
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(4294967280))); // 0xF0
    }

    [Test]
    public void Test_Not_Imm()
    {
        var asm = @"
                not r1, 15
                out r1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(4294967280))); // 0xF0
    }

    [Test]
    public void Test_Push_Pop()
    {
        var asm = @"
                mov r1, 119
                push r1
                mov r1, 0
                pop r2
                out r2
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(119))); // 0x77
    }

    [Test]
    public void Test_Call_Ret()
    {
        var asm = @"
                call subroutine
                out 0
                halt:
                jmp halt
                subroutine:
                out 170
                ret
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(170))); // 0xAA
    }

    [Test]
    public void Test_Add_Overflow()
    {
        var asm = @"
                mov r1, 255
                add r1, r2, 1
                out r2
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(0)));
    }

    [Test]
    public void Test_Sub_Borrow()
    {
        var asm = @"
                mov r2, 0
                sub r1, r2, 1
                out r1
            ";
        var cpu = Run(asm);
        Assert.That(cpu.Output, Is.EqualTo(new Int(4294967295)));
    }

    [Test]
    public void Test_Does_Power()
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

        var cpu = Run(asm, new Byte(2), new Byte(4));
        // 2^4 = 16
        Assert.That(cpu.Output, Is.EqualTo(new Int(16)));
    }
}