using Turing.Core.Overture;
namespace Turing.Tests.Overture;

[TestFixture]
internal class ALUTests
{
    // Opcode mapping: 0=NAND, 1=OR, 2=AND, 3=NOR, 4=ADD, 5=SUB
    [TestCase(0, 0xAA, 0xCC, 0x77)]  // NAND
    [TestCase(1, 0xAA, 0xCC, 0xEE)]  // OR
    [TestCase(2, 0xAA, 0xCC, 0x88)]  // AND
    [TestCase(3, 0xAA, 0xCC, 0x11)]  // NOR
    [TestCase(4, 0xAA, 0xCC, 0x76)]  // ADD: 0xAA + 0xCC = 0x176 -> low byte 0x76
    [TestCase(5, 0xAA, 0xCC, 0xDE)]  // SUB: 0xAA - 0xCC = -0x22 -> two's complement 0xDE
    public void ALU_ComputesCorrectOperation(int opcode, int a, int b, int expected)
    {
        Byte op = CreateOpcodeByte(opcode);
        Byte operandA = new Byte(a);
        Byte operandB = new Byte(b);
        Byte expectedResult = new Byte(expected);
        ALU alu = new ALU(op, operandA, operandB);
        Byte result = (Byte)alu;
        Assert.That(result, Is.EqualTo(expectedResult),
            $"Opcode {opcode}: expected 0x{expected:X2}, got 0x{result.ToHexString()}");
    }

    [Test]
    public void ALU_Addition_WrapsAround()
    {
        ALU alu = new ALU(CreateOpcodeByte(4), new Byte(0xFF), new Byte(0x01));
        Byte result = (Byte)alu;
        Assert.That(result, Is.EqualTo(new Byte(0x00)),
            $"0xFF + 0x01 should wrap to 0x00, got 0x{result.ToHexString()}");
    }

    [Test]
    public void ALU_Subtraction_WithBorrow()
    {
        ALU alu = new ALU(CreateOpcodeByte(5), new Byte(0x00), new Byte(0x01));
        Byte result = (Byte)alu;
        Assert.That(result, Is.EqualTo(new Byte(0xFF)),
            $"0x00 - 0x01 should be 0xFF, got 0x{result.ToHexString()}");
    }

    [Test]
    public void ALU_UnsupportedOpcode_ReturnsZero()
    {
        for (int opcode = 6; opcode <= 7; opcode++)
        {
            Byte op = CreateOpcodeByte(opcode);
            ALU alu = new ALU(op, new Byte(0xAA), new Byte(0xCC));
            Byte result = (Byte)alu;
            Assert.That(result, Is.EqualTo(new Byte(0x00)),
                $"Opcode {opcode} should return 0");
        }
    }

    // ==========================================
    // EXHAUSTIVE TEST: all 256x256 inputs for each opcode
    // ==========================================

    [Test]
    public void ALU_Exhaustive_AllOpcodes_AllInputs()
    {
        for (int opcode = 0; opcode <= 5; opcode++)
        {
            for (int a = 0; a < 256; a++)
            {
                for (int b = 0; b < 256; b++)
                {
                    Byte op = CreateOpcodeByte(opcode);
                    Byte operandA = new Byte(a);
                    Byte operandB = new Byte(b);
                    ALU alu = new ALU(op, operandA, operandB);
                    Byte result = (Byte)alu;
                    byte expected = ComputeExpected(opcode, (byte)a, (byte)b);
                    Assert.That((byte)result, Is.EqualTo(expected),
                        $"Opcode {opcode}, a={a:X2}, b={b:X2}: expected 0x{expected:X2}, got 0x{result.ToHexString()}");
                }
            }
        }
    }

    // ==========================================
    // Helpers
    // ==========================================

    private static Byte CreateOpcodeByte(int opcode)
    {
        // Map opcode to bits 5,6,7
        int value = (opcode & 1) << 5 |
                    ((opcode >> 1) & 1) << 6 |
                    ((opcode >> 2) & 1) << 7;
        return new Byte(value);
    }

    private static byte ComputeExpected(int opcode, byte a, byte b)
    {
        return opcode switch
        {
            0 => (byte)~(a & b),           // NAND
            1 => (byte)(a | b),            // OR
            2 => (byte)(a & b),            // AND
            3 => (byte)~(a | b),           // NOR
            4 => (byte)(a + b),            // ADD (low byte)
            5 => (byte)(a - b),            // SUB (two's complement)
            _ => 0
        };
    }
}