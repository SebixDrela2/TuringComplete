using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Tests.Gates;

[TestFixture]
internal class XORTests
{
    // ==========================================
    // BIT TESTS
    // ==========================================

    [TestCase(0, 0, 0)]
    [TestCase(0, 1, 1)]
    [TestCase(1, 0, 1)]
    [TestCase(1, 1, 0)]
    public void XOR_ImplicitConversion_WithBitInputs_ReturnsCorrectOutput(int inputA, int inputB, int expectedOutputInt)
    {
        // Arrange
        var a = new Bit(inputA);
        var b = new Bit(inputB);
        var expected = new Bit(expectedOutputInt);

        // Act
        Bit actual = new XOR<Bit>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // BYTE TESTS
    // ==========================================

    [Test]
    public void XOR_ImplicitConversion_WithByteInputs_ReturnsBitwiseXOR()
    {
        // Arrange
        var a = new Byte(0xAA); // 10101010
        var b = new Byte(0xCC); // 11001100
        var expected = new Byte(0x66); // 01100110

        // Act
        Byte actual = new XOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XOR_ImplicitConversion_WithByteInputs_SameValue_ReturnsZero()
    {
        // Arrange
        var a = new Byte(0xAA);
        var b = new Byte(0xAA);
        var expected = new Byte(0x00);

        // Act
        Byte actual = new XOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XOR_ImplicitConversion_WithByteInputs_Complement_ReturnsAllOnes()
    {
        // Arrange
        var a = new Byte(0xAA);
        var b = new Byte(0x55);
        var expected = new Byte(0xFF);

        // Act
        Byte actual = new XOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XOR_ImplicitConversion_WithByteInputs_OneZero_ReturnsOriginal()
    {
        // Arrange
        var a = new Byte(0xAA);
        var b = new Byte(0x00);
        var expected = new Byte(0xAA);

        // Act
        Byte actual = new XOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XOR_ImplicitConversion_WithByteInputs_ZeroZero_ReturnsZero()
    {
        // Arrange
        var a = new Byte(0x00);
        var b = new Byte(0x00);
        var expected = new Byte(0x00);

        // Act
        Byte actual = new XOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XOR_ImplicitConversion_WithByteInputs_Alternating_ReturnsCorrect()
    {
        // Arrange
        var a = new Byte(0b10101010); // 0xAA
        var b = new Byte(0b11001100); // 0xCC
        var expected = new Byte(0b01100110); // 0x66

        // Act
        Byte actual = new XOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // SHORT TESTS
    // ==========================================

    [Test]
    public void XOR_ImplicitConversion_WithShortInputs_ReturnsBitwiseXOR()
    {
        // Arrange
        var a = new Short(0xAAAA);
        var b = new Short(0xCCCC);
        var expected = new Short(0x6666); // 0xAAAA ^ 0xCCCC = 0x6666

        // Act
        Short actual = new XOR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XOR_ImplicitConversion_WithShortInputs_SameValue_ReturnsZero()
    {
        // Arrange
        var a = new Short(0xAAAA);
        var b = new Short(0xAAAA);
        var expected = new Short(0x0000);

        // Act
        Short actual = new XOR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XOR_ImplicitConversion_WithShortInputs_Complement_ReturnsAllOnes()
    {
        // Arrange
        var a = new Short(0xAAAA);
        var b = new Short(0x5555);
        var expected = new Short(0xFFFF);

        // Act
        Short actual = new XOR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XOR_ImplicitConversion_WithShortInputs_OneZero_ReturnsOriginal()
    {
        // Arrange
        var a = new Short(0xAAAA);
        var b = new Short(0x0000);
        var expected = new Short(0xAAAA);

        // Act
        Short actual = new XOR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // INT TESTS
    // ==========================================

    [Test]
    public void XOR_ImplicitConversion_WithIntInputs_ReturnsBitwiseXOR()
    {
        // Arrange
        var a = new Int(0xAAAAAAAA);
        var b = new Int(0xCCCCCCCC);
        var expected = new Int(0x66666666); // 0xAAAAAAAA ^ 0xCCCCCCCC = 0x66666666

        // Act
        Int actual = new XOR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XOR_ImplicitConversion_WithIntInputs_SameValue_ReturnsZero()
    {
        // Arrange
        var a = new Int(0xAAAAAAAA);
        var b = new Int(0xAAAAAAAA);
        var expected = new Int(0x00000000);

        // Act
        Int actual = new XOR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XOR_ImplicitConversion_WithIntInputs_Complement_ReturnsAllOnes()
    {
        // Arrange
        var a = new Int(0xAAAAAAAA);
        var b = new Int(0x55555555);
        var expected = new Int(0xFFFFFFFF);

        // Act
        Int actual = new XOR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XOR_ImplicitConversion_WithIntInputs_OneZero_ReturnsOriginal()
    {
        // Arrange
        var a = new Int(0xAAAAAAAA);
        var b = new Int(0x00000000);
        var expected = new Int(0xAAAAAAAA);

        // Act
        Int actual = new XOR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // LONG TESTS
    // ==========================================

    [Test]
    public void XOR_ImplicitConversion_WithLongInputs_ReturnsBitwiseXOR()
    {
        // Arrange
        var a = new Long(0xAAAAAAAAAAAAAAAAL);
        var b = new Long(0xCCCCCCCCCCCCCCCCL);
        var expected = new Long(0x6666666666666666L); // 0xAAAAAAAAAAAAAAAA ^ 0xCCCCCCCCCCCCCCCC = 0x6666666666666666

        // Act
        Long actual = new XOR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XOR_ImplicitConversion_WithLongInputs_SameValue_ReturnsZero()
    {
        // Arrange
        var a = new Long(0xAAAAAAAAAAAAAAAAL);
        var b = new Long(0xAAAAAAAAAAAAAAAAL);
        var expected = new Long(0x0000000000000000L);

        // Act
        Long actual = new XOR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XOR_ImplicitConversion_WithLongInputs_Complement_ReturnsAllOnes()
    {
        // Arrange
        var a = new Long(0xAAAAAAAAAAAAAAAAL);
        var b = new Long(0x5555555555555555L);
        var expected = new Long(0xFFFFFFFFFFFFFFFFL);

        // Act
        Long actual = new XOR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XOR_ImplicitConversion_WithLongInputs_OneZero_ReturnsOriginal()
    {
        // Arrange
        var a = new Long(0xAAAAAAAAAAAAAAAAL);
        var b = new Long(0x0000000000000000L);
        var expected = new Long(0xAAAAAAAAAAAAAAAAL);

        // Act
        Long actual = new XOR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // LSB ONLY TESTS (Single bit behavior)
    // ==========================================

    [TestCase(0, 0, 0)]
    [TestCase(0, 1, 1)]
    [TestCase(1, 0, 1)]
    [TestCase(1, 1, 0)]
    public void XOR_ImplicitConversion_WithByteInputs_ReturnsCorrectLSB(int inputA, int inputB, int expectedOutputInt)
    {
        // Arrange
        var a = new Byte(inputA);
        var b = new Byte(inputB);
        var expected = new Bit(expectedOutputInt);

        // Act
        Byte actualByte = new XOR<Byte>(a, b);
        Bit actual = new Bit((bool)actualByte.GetBit(0));

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}