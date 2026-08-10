using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Tests.Gates;

[TestFixture]
internal class NANDTests
{
    [TestCase(0, 0, 1)]
    [TestCase(0, 1, 1)]
    [TestCase(1, 0, 1)]
    [TestCase(1, 1, 0)]
    public void NAND_ImplicitConversion_WithBitInputs_ReturnsCorrectOutput(int inputA, int inputB, int expectedOutputInt)
    {
        // Arrange
        var a = new Bit(inputA);
        var b = new Bit(inputB);
        var nand = new NAND<Bit>(a, b);
        var expected = new Bit(expectedOutputInt);

        // Act
        Bit actual = nand;

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // BYTE TESTS
    [Test]
    public void NAND_ImplicitConversion_WithByteInputs_ReturnsBitwiseNAND()
    {
        // Arrange
        var a = new Byte(0b10101010); // 0xAA
        var b = new Byte(0b11001100); // 0xCC
        // 0xAA & 0xCC = 0x88, NAND = ~0x88 = 0x77
        var expected = new Byte(0x77);

        // Act
        Byte actual = new NAND<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NAND_ImplicitConversion_WithByteInputs_AllOnes_ReturnsZero()
    {
        // Arrange
        var a = new Byte(0xFF);
        var b = new Byte(0xFF);
        var expected = new Byte(0x00);

        // Act
        Byte actual = new NAND<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NAND_ImplicitConversion_WithByteInputs_OneZero_ReturnsAllOnes()
    {
        // Arrange
        var a = new Byte(0xFF);
        var b = new Byte(0x00);
        var expected = new Byte(0xFF);

        // Act
        Byte actual = new NAND<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NAND_ImplicitConversion_WithShortInputs_ReturnsBitwiseNAND()
    {
        // Arrange
        var a = new Short(0xAAAA);
        var b = new Short(0xCCCC);
        var expected = new Short(0x7777);

        // Act
        Short actual = new NAND<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NAND_ImplicitConversion_WithShortInputs_AllOnes_ReturnsZero()
    {
        // Arrange
        var a = new Short(0xFFFF);
        var b = new Short(0xFFFF);
        var expected = new Short(0x0000);

        // Act
        Short actual = new NAND<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // INT TESTS
    [Test]
    public void NAND_ImplicitConversion_WithIntInputs_ReturnsBitwiseNAND()
    {
        // Arrange
        var a = new Int(0xAAAAAAAA);
        var b = new Int(0xCCCCCCCC);
        var expected = new Int(0x77777777);

        // Act
        Int actual = new NAND<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NAND_ImplicitConversion_WithIntInputs_AllOnes_ReturnsZero()
    {
        // Arrange
        var a = new Int(0xFFFFFFFF);
        var b = new Int(0xFFFFFFFF);
        var expected = new Int(0x00000000);

        // Act
        Int actual = new NAND<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // LONG TESTS
    [Test]
    public void NAND_ImplicitConversion_WithLongInputs_ReturnsBitwiseNAND()
    {
        // Arrange
        var a = new Long(0xAAAAAAAAAAAAAAAAL);
        var b = new Long(0xCCCCCCCCCCCCCCCCL);
        var expected = new Long(0x7777777777777777L);

        // Act
        Long actual = new NAND<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NAND_ImplicitConversion_WithLongInputs_AllOnes_ReturnsZero()
    {
        // Arrange
        var a = new Long(0xFFFFFFFFFFFFFFFFL);
        var b = new Long(0xFFFFFFFFFFFFFFFFL);
        var expected = new Long(0x0000000000000000L);

        // Act
        Long actual = new NAND<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase(0, 0, 1)]
    [TestCase(0, 1, 1)]
    [TestCase(1, 0, 1)]
    [TestCase(1, 1, 0)]
    public void NAND_ImplicitConversion_WithByteInputs_ReturnsCorrectLSB(int inputA, int inputB, int expectedOutputInt)
    {
        // Arrange
        var a = new Byte(inputA);
        var b = new Byte(inputB);
        var expected = new Bit(expectedOutputInt);

        // Act
        Byte actualByte = new NAND<Byte>(a, b);
        Bit actual = new Bit((bool)actualByte.GetBit(0));

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}