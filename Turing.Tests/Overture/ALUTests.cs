using Turing.Core.Overture;

namespace Turing.Tests.Overture;

[TestFixture]
internal class ALUTests
{
    // Opcode 0: NAND, 1: OR, 2: AND, 3: NOR
    [TestCase(0, 0xAA, 0xCC, 0x77)]  // NAND: ~(0xAA & 0xCC) = ~0x88 = 0x77
    [TestCase(1, 0xAA, 0xCC, 0xEE)]  // OR:  0xAA | 0xCC = 0xEE
    [TestCase(2, 0xAA, 0xCC, 0x88)]  // AND: 0xAA & 0xCC = 0x88
    [TestCase(3, 0xAA, 0xCC, 0x11)]  // NOR: ~(0xAA | 0xCC) = ~0xEE = 0x11
    public void ALU_ComputesCorrectOperation(int opcode, int a, int b, int expected)
    {
        // Arrange
        Byte op = CreateOpcodeByte(opcode);
        Byte operandA = new Byte(a);
        Byte operandB = new Byte(b);
        Byte expectedResult = new Byte(expected);

        // Act
        ALU alu = new ALU(op, operandA, operandB);
        Byte result = (Byte)alu;

        // Assert
        Assert.That(result, Is.EqualTo(expectedResult),
            $"Opcode {opcode}: expected 0x{expected:X2}, got 0x{result.ToHexString()}");
    }

    [Test]
    public void ALU_OpcodeBits5_6_7_AreUsed()
    {
        // Opcode 1 (OR) at bits 5,6,7 = 0b001 -> bit5=1
        for (int mask = 0; mask < 32; mask++)
        {
            int opByteValue = (1 << 5) | mask;
            Byte op = new Byte(opByteValue);
            Byte a = new Byte(0xAA);
            Byte b = new Byte(0xCC);
            ALU alu = new ALU(op, a, b);
            Byte result = (Byte)alu;
            Assert.That(result, Is.EqualTo(new Byte(0xEE)),
                $"Mask {mask} failed, got {result.ToHexString()}");
        }
    }

    [Test]
    public void ALU_UnsupportedOpcode_ReturnsZero()
    {
        for (int opcode = 4; opcode <= 7; opcode++)
        {
            Byte op = CreateOpcodeByte(opcode);
            Byte a = new Byte(0xAA);
            Byte b = new Byte(0xCC);
            ALU alu = new ALU(op, a, b);
            Byte result = (Byte)alu;
            Assert.That(result, Is.EqualTo(new Byte(0x00)),
                $"Opcode {opcode} should return 0");
        }
    }

    private static Byte CreateOpcodeByte(int opcode)
    {
        int value = (opcode & 1) << 5 |
                    ((opcode >> 1) & 1) << 6 |
                    ((opcode >> 2) & 1) << 7;
        return new Byte(value);
    }
}