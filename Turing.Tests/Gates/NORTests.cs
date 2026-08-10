using Turing.Core.Electricity;
using Turing.Core.Gates;

namespace Turing.Tests.Gates;

[TestFixture]
internal class NORTests
{
    // ==========================================
    // BIT TESTS
    // ==========================================

    [TestCase(0, 0, 1)]
    [TestCase(0, 1, 0)]
    [TestCase(1, 0, 0)]
    [TestCase(1, 1, 0)]
    public void NOR_ImplicitConversion_WithBitInputs_ReturnsCorrectOutput(int inputA, int inputB, int expectedOutputInt)
    {
        // Arrange
        var a = new Bit(inputA);
        var b = new Bit(inputB);
        var expected = new Bit(expectedOutputInt);

        // Act
        Bit actual = new NOR<Bit>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // BYTE TESTS
    // ==========================================

    [Test]
    public void NOR_ImplicitConversion_WithByteInputs_ReturnsBitwiseNOR()
    {
        // Arrange
        var a = new Byte(0xAA); // 10101010
        var b = new Byte(0xCC); // 11001100
        // OR = 0xEE (11101110), NOR = ~0xEE = 0x11 (00010001)
        var expected = new Byte(0x11);

        // Act
        Byte actual = new NOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOR_ImplicitConversion_WithByteInputs_AllOnes_ReturnsZero()
    {
        // Arrange
        var a = new Byte(0xFF);
        var b = new Byte(0xFF);
        // OR = 0xFF, NOR = ~0xFF = 0x00
        var expected = new Byte(0x00);

        // Act
        Byte actual = new NOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOR_ImplicitConversion_WithByteInputs_OneZero_ReturnsZero()
    {
        // Arrange
        var a = new Byte(0xFF);
        var b = new Byte(0x00);
        // OR = 0xFF, NOR = ~0xFF = 0x00
        var expected = new Byte(0x00);

        // Act
        Byte actual = new NOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOR_ImplicitConversion_WithByteInputs_ZeroZero_ReturnsAllOnes()
    {
        // Arrange
        var a = new Byte(0x00);
        var b = new Byte(0x00);
        // OR = 0x00, NOR = ~0x00 = 0xFF
        var expected = new Byte(0xFF);

        // Act
        Byte actual = new NOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOR_ImplicitConversion_WithByteInputs_Alternating_ReturnsCorrect()
    {
        // Arrange
        var a = new Byte(0b10101010); // 0xAA
        var b = new Byte(0b01010101); // 0x55
        // OR = 0b11111111 (0xFF), NOR = 0b00000000 (0x00)
        var expected = new Byte(0b00000000);

        // Act
        Byte actual = new NOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOR_ImplicitConversion_WithByteInputs_Alternating2_ReturnsCorrect()
    {
        // Arrange
        var a = new Byte(0b10101010); // 0xAA
        var b = new Byte(0b11001100); // 0xCC
        // OR = 0b11101110 (0xEE), NOR = 0b00010001 (0x11)
        var expected = new Byte(0b00010001);

        // Act
        Byte actual = new NOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // SHORT TESTS
    // ==========================================

    [Test]
    public void NOR_ImplicitConversion_WithShortInputs_ReturnsBitwiseNOR()
    {
        // Arrange
        var a = new Short(0xAAAA);
        var b = new Short(0xCCCC);
        // OR = 0xEEEE, NOR = ~0xEEEE = 0x1111
        var expected = new Short(0x1111);

        // Act
        Short actual = new NOR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOR_ImplicitConversion_WithShortInputs_AllOnes_ReturnsZero()
    {
        // Arrange
        var a = new Short(0xFFFF);
        var b = new Short(0xFFFF);
        var expected = new Short(0x0000);

        // Act
        Short actual = new NOR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOR_ImplicitConversion_WithShortInputs_OneZero_ReturnsZero()
    {
        // Arrange
        var a = new Short(0xFFFF);
        var b = new Short(0x0000);
        var expected = new Short(0x0000);

        // Act
        Short actual = new NOR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOR_ImplicitConversion_WithShortInputs_ZeroZero_ReturnsAllOnes()
    {
        // Arrange
        var a = new Short(0x0000);
        var b = new Short(0x0000);
        var expected = new Short(0xFFFF);

        // Act
        Short actual = new NOR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // INT TESTS
    // ==========================================

    [Test]
    public void NOR_ImplicitConversion_WithIntInputs_ReturnsBitwiseNOR()
    {
        // Arrange
        var a = new Int(0xAAAAAAAA);
        var b = new Int(0xCCCCCCCC);
        // OR = 0xEEEEEEEE, NOR = ~0xEEEEEEEE = 0x11111111
        var expected = new Int(0x11111111);

        // Act
        Int actual = new NOR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOR_ImplicitConversion_WithIntInputs_AllOnes_ReturnsZero()
    {
        // Arrange
        var a = new Int(0xFFFFFFFF);
        var b = new Int(0xFFFFFFFF);
        var expected = new Int(0x00000000);

        // Act
        Int actual = new NOR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOR_ImplicitConversion_WithIntInputs_OneZero_ReturnsZero()
    {
        // Arrange
        var a = new Int(0xFFFFFFFF);
        var b = new Int(0x00000000);
        var expected = new Int(0x00000000);

        // Act
        Int actual = new NOR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOR_ImplicitConversion_WithIntInputs_ZeroZero_ReturnsAllOnes()
    {
        // Arrange
        var a = new Int(0x00000000);
        var b = new Int(0x00000000);
        var expected = new Int(0xFFFFFFFF);

        // Act
        Int actual = new NOR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // LONG TESTS
    // ==========================================

    [Test]
    public void NOR_ImplicitConversion_WithLongInputs_ReturnsBitwiseNOR()
    {
        // Arrange
        var a = new Long(0xAAAAAAAAAAAAAAAAL);
        var b = new Long(0xCCCCCCCCCCCCCCCCL);
        // OR = 0xEEEEEEEEEEEEEEEE, NOR = ~0xEEEEEEEEEEEEEEEE = 0x1111111111111111
        var expected = new Long(0x1111111111111111L);

        // Act
        Long actual = new NOR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOR_ImplicitConversion_WithLongInputs_AllOnes_ReturnsZero()
    {
        // Arrange
        var a = new Long(0xFFFFFFFFFFFFFFFFL);
        var b = new Long(0xFFFFFFFFFFFFFFFFL);
        var expected = new Long(0x0000000000000000L);

        // Act
        Long actual = new NOR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOR_ImplicitConversion_WithLongInputs_OneZero_ReturnsZero()
    {
        // Arrange
        var a = new Long(0xFFFFFFFFFFFFFFFFL);
        var b = new Long(0x0000000000000000L);
        var expected = new Long(0x0000000000000000L);

        // Act
        Long actual = new NOR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOR_ImplicitConversion_WithLongInputs_ZeroZero_ReturnsAllOnes()
    {
        // Arrange
        var a = new Long(0x0000000000000000L);
        var b = new Long(0x0000000000000000L);
        var expected = new Long(0xFFFFFFFFFFFFFFFFL);

        // Act
        Long actual = new NOR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // LSB ONLY TESTS (Single bit behavior)
    // ==========================================

    [TestCase(0, 0, 1)]
    [TestCase(0, 1, 0)]
    [TestCase(1, 0, 0)]
    [TestCase(1, 1, 0)]
    public void NOR_ImplicitConversion_WithByteInputs_ReturnsCorrectLSB(int inputA, int inputB, int expectedOutputInt)
    {
        // Arrange
        var a = new Byte(inputA);
        var b = new Byte(inputB);
        var expected = new Bit(expectedOutputInt);

        // Act
        Byte actualByte = new NOR<Byte>(a, b);
        Bit actual = new Bit(actualByte.GetBit(0));

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}