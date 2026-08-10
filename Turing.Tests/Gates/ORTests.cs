using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Tests.Gates;

[TestFixture]
internal class ORTests
{
    // ==========================================
    // BIT TESTS
    // ==========================================

    [TestCase(0, 0, 0)]
    [TestCase(0, 1, 1)]
    [TestCase(1, 0, 1)]
    [TestCase(1, 1, 1)]
    public void OR_ImplicitConversion_WithBitInputs_ReturnsCorrectOutput(int inputA, int inputB, int expectedOutputInt)
    {
        // Arrange
        var a = new Bit(inputA);
        var b = new Bit(inputB);
        var expected = new Bit(expectedOutputInt);

        // Act
        Bit actual = new OR<Bit>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // BYTE TESTS
    // ==========================================

    [Test]
    public void OR_ImplicitConversion_WithByteInputs_ReturnsBitwiseOR()
    {
        // Arrange
        var a = new Byte(0xAA); // 10101010
        var b = new Byte(0xCC); // 11001100
        var expected = new Byte(0xEE); // 11101110

        // Act
        Byte actual = new OR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OR_ImplicitConversion_WithByteInputs_AllOnes_ReturnsAllOnes()
    {
        // Arrange
        var a = new Byte(0xFF);
        var b = new Byte(0xFF);
        var expected = new Byte(0xFF);

        // Act
        Byte actual = new OR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OR_ImplicitConversion_WithByteInputs_OneZero_ReturnsAllOnes()
    {
        // Arrange
        var a = new Byte(0xFF);
        var b = new Byte(0x00);
        var expected = new Byte(0xFF);

        // Act
        Byte actual = new OR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OR_ImplicitConversion_WithByteInputs_ZeroZero_ReturnsZero()
    {
        // Arrange
        var a = new Byte(0x00);
        var b = new Byte(0x00);
        var expected = new Byte(0x00);

        // Act
        Byte actual = new OR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OR_ImplicitConversion_WithByteInputs_Alternating_ReturnsCorrect()
    {
        // Arrange
        var a = new Byte(0b10101010); // 0xAA
        var b = new Byte(0b01010101); // 0x55
        var expected = new Byte(0b11111111); // 0xFF

        // Act
        Byte actual = new OR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // SHORT TESTS
    // ==========================================

    [Test]
    public void OR_ImplicitConversion_WithShortInputs_ReturnsBitwiseOR()
    {
        // Arrange
        var a = new Short(0xAAAA);
        var b = new Short(0xCCCC);
        var expected = new Short(0xEEEE); // 0xAAAA | 0xCCCC = 0xEEEE

        // Act
        Short actual = new OR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OR_ImplicitConversion_WithShortInputs_AllOnes_ReturnsAllOnes()
    {
        // Arrange
        var a = new Short(0xFFFF);
        var b = new Short(0xFFFF);
        var expected = new Short(0xFFFF);

        // Act
        Short actual = new OR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OR_ImplicitConversion_WithShortInputs_OneZero_ReturnsAllOnes()
    {
        // Arrange
        var a = new Short(0xFFFF);
        var b = new Short(0x0000);
        var expected = new Short(0xFFFF);

        // Act
        Short actual = new OR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OR_ImplicitConversion_WithShortInputs_ZeroZero_ReturnsZero()
    {
        // Arrange
        var a = new Short(0x0000);
        var b = new Short(0x0000);
        var expected = new Short(0x0000);

        // Act
        Short actual = new OR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // INT TESTS
    // ==========================================

    [Test]
    public void OR_ImplicitConversion_WithIntInputs_ReturnsBitwiseOR()
    {
        // Arrange
        var a = new Int(0xAAAAAAAA);
        var b = new Int(0xCCCCCCCC);
        var expected = new Int(0xEEEEEEEE); // 0xAAAAAAAA | 0xCCCCCCCC = 0xEEEEEEEE

        // Act
        Int actual = new OR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OR_ImplicitConversion_WithIntInputs_AllOnes_ReturnsAllOnes()
    {
        // Arrange
        var a = new Int(0xFFFFFFFF);
        var b = new Int(0xFFFFFFFF);
        var expected = new Int(0xFFFFFFFF);

        // Act
        Int actual = new OR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OR_ImplicitConversion_WithIntInputs_OneZero_ReturnsAllOnes()
    {
        // Arrange
        var a = new Int(0xFFFFFFFF);
        var b = new Int(0x00000000);
        var expected = new Int(0xFFFFFFFF);

        // Act
        Int actual = new OR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OR_ImplicitConversion_WithIntInputs_ZeroZero_ReturnsZero()
    {
        // Arrange
        var a = new Int(0x00000000);
        var b = new Int(0x00000000);
        var expected = new Int(0x00000000);

        // Act
        Int actual = new OR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // LONG TESTS
    // ==========================================

    [Test]
    public void OR_ImplicitConversion_WithLongInputs_ReturnsBitwiseOR()
    {
        // Arrange
        var a = new Long(0xAAAAAAAAAAAAAAAAL);
        var b = new Long(0xCCCCCCCCCCCCCCCCL);
        var expected = new Long(0xEEEEEEEEEEEEEEEEL); // 0xAAAAAAAAAAAAAAAA | 0xCCCCCCCCCCCCCCCC = 0xEEEEEEEEEEEEEEEE

        // Act
        Long actual = new OR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OR_ImplicitConversion_WithLongInputs_AllOnes_ReturnsAllOnes()
    {
        // Arrange
        var a = new Long(0xFFFFFFFFFFFFFFFFL);
        var b = new Long(0xFFFFFFFFFFFFFFFFL);
        var expected = new Long(0xFFFFFFFFFFFFFFFFL);

        // Act
        Long actual = new OR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OR_ImplicitConversion_WithLongInputs_OneZero_ReturnsAllOnes()
    {
        // Arrange
        var a = new Long(0xFFFFFFFFFFFFFFFFL);
        var b = new Long(0x0000000000000000L);
        var expected = new Long(0xFFFFFFFFFFFFFFFFL);

        // Act
        Long actual = new OR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void OR_ImplicitConversion_WithLongInputs_ZeroZero_ReturnsZero()
    {
        // Arrange
        var a = new Long(0x0000000000000000L);
        var b = new Long(0x0000000000000000L);
        var expected = new Long(0x0000000000000000L);

        // Act
        Long actual = new OR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // LSB ONLY TESTS (Single bit behavior)
    // ==========================================

    [TestCase(0, 0, 0)]
    [TestCase(0, 1, 1)]
    [TestCase(1, 0, 1)]
    [TestCase(1, 1, 1)]
    public void OR_ImplicitConversion_WithByteInputs_ReturnsCorrectLSB(int inputA, int inputB, int expectedOutputInt)
    {
        // Arrange
        var a = new Byte(inputA);
        var b = new Byte(inputB);
        var expected = new Bit(expectedOutputInt);

        // Act
        Byte actualByte = new OR<Byte>(a, b);
        Bit actual = new Bit(actualByte.GetBit(0));

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}