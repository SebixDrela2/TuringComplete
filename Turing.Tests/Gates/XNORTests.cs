using Turing.Core.Gates;

namespace Turing.Tests.Gates;

[TestFixture]
internal class XNORTests
{
    // ==========================================
    // BIT TESTS
    // ==========================================

    [TestCase(0, 0, 1)]
    [TestCase(0, 1, 0)]
    [TestCase(1, 0, 0)]
    [TestCase(1, 1, 1)]
    public void XNOR_ImplicitConversion_WithBitInputs_ReturnsCorrectOutput(int inputA, int inputB, int expectedOutputInt)
    {
        // Arrange
        var a = new Bit(inputA);
        var b = new Bit(inputB);
        var expected = new Bit(expectedOutputInt);

        // Act
        Bit actual = new XNOR<Bit>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // BYTE TESTS
    // ==========================================

    [Test]
    public void XNOR_ImplicitConversion_WithByteInputs_ReturnsBitwiseXNOR()
    {
        // Arrange
        var a = new Byte(0xAA); // 10101010
        var b = new Byte(0xCC); // 11001100
        // XOR = 0x66 (01100110), XNOR = ~0x66 = 0x99 (10011001)
        var expected = new Byte(0x99);

        // Act
        Byte actual = new XNOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XNOR_ImplicitConversion_WithByteInputs_SameValue_ReturnsAllOnes()
    {
        // Arrange
        var a = new Byte(0xAA);
        var b = new Byte(0xAA);
        var expected = new Byte(0xFF);

        // Act
        Byte actual = new XNOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XNOR_ImplicitConversion_WithByteInputs_Complement_ReturnsZero()
    {
        // Arrange
        var a = new Byte(0xAA);
        var b = new Byte(0x55);
        var expected = new Byte(0x00);

        // Act
        Byte actual = new XNOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XNOR_ImplicitConversion_WithByteInputs_OneZero_ReturnsComplement()
    {
        // Arrange
        var a = new Byte(0xAA);
        var b = new Byte(0x00);
        var expected = new Byte(0x55); // ~0xAA

        // Act
        Byte actual = new XNOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XNOR_ImplicitConversion_WithByteInputs_ZeroZero_ReturnsAllOnes()
    {
        // Arrange
        var a = new Byte(0x00);
        var b = new Byte(0x00);
        var expected = new Byte(0xFF);

        // Act
        Byte actual = new XNOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XNOR_ImplicitConversion_WithByteInputs_Alternating_ReturnsCorrect()
    {
        // Arrange
        var a = new Byte(0b10101010); // 0xAA
        var b = new Byte(0b11001100); // 0xCC
        // XOR = 0b01100110 (0x66), XNOR = ~0x66 = 0b10011001 (0x99)
        var expected = new Byte(0b10011001);

        // Act
        Byte actual = new XNOR<Byte>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // SHORT TESTS
    // ==========================================

    [Test]
    public void XNOR_ImplicitConversion_WithShortInputs_ReturnsBitwiseXNOR()
    {
        // Arrange
        var a = new Short(0xAAAA);
        var b = new Short(0xCCCC);
        // XOR = 0x6666, XNOR = ~0x6666 = 0x9999
        var expected = new Short(0x9999);

        // Act
        Short actual = new XNOR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XNOR_ImplicitConversion_WithShortInputs_SameValue_ReturnsAllOnes()
    {
        // Arrange
        var a = new Short(0xAAAA);
        var b = new Short(0xAAAA);
        var expected = new Short(0xFFFF);

        // Act
        Short actual = new XNOR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XNOR_ImplicitConversion_WithShortInputs_Complement_ReturnsZero()
    {
        // Arrange
        var a = new Short(0xAAAA);
        var b = new Short(0x5555);
        var expected = new Short(0x0000);

        // Act
        Short actual = new XNOR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XNOR_ImplicitConversion_WithShortInputs_OneZero_ReturnsComplement()
    {
        // Arrange
        var a = new Short(0xAAAA);
        var b = new Short(0x0000);
        var expected = new Short(0x5555);

        // Act
        Short actual = new XNOR<Short>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // INT TESTS
    // ==========================================

    [Test]
    public void XNOR_ImplicitConversion_WithIntInputs_ReturnsBitwiseXNOR()
    {
        // Arrange
        var a = new Int(0xAAAAAAAA);
        var b = new Int(0xCCCCCCCC);
        // XOR = 0x66666666, XNOR = ~0x66666666 = 0x99999999
        var expected = new Int(0x99999999);

        // Act
        Int actual = new XNOR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XNOR_ImplicitConversion_WithIntInputs_SameValue_ReturnsAllOnes()
    {
        // Arrange
        var a = new Int(0xAAAAAAAA);
        var b = new Int(0xAAAAAAAA);
        var expected = new Int(0xFFFFFFFF);

        // Act
        Int actual = new XNOR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XNOR_ImplicitConversion_WithIntInputs_Complement_ReturnsZero()
    {
        // Arrange
        var a = new Int(0xAAAAAAAA);
        var b = new Int(0x55555555);
        var expected = new Int(0x00000000);

        // Act
        Int actual = new XNOR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XNOR_ImplicitConversion_WithIntInputs_OneZero_ReturnsComplement()
    {
        // Arrange
        var a = new Int(0xAAAAAAAA);
        var b = new Int(0x00000000);
        var expected = new Int(0x55555555);

        // Act
        Int actual = new XNOR<Int>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // LONG TESTS
    // ==========================================

    [Test]
    public void XNOR_ImplicitConversion_WithLongInputs_ReturnsBitwiseXNOR()
    {
        // Arrange
        var a = new Long(0xAAAAAAAAAAAAAAAAL);
        var b = new Long(0xCCCCCCCCCCCCCCCCL);
        // XOR = 0x6666666666666666, XNOR = ~0x6666666666666666 = 0x9999999999999999
        var expected = new Long(0x9999999999999999L);

        // Act
        Long actual = new XNOR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XNOR_ImplicitConversion_WithLongInputs_SameValue_ReturnsAllOnes()
    {
        // Arrange
        var a = new Long(0xAAAAAAAAAAAAAAAAL);
        var b = new Long(0xAAAAAAAAAAAAAAAAL);
        var expected = new Long(0xFFFFFFFFFFFFFFFFL);

        // Act
        Long actual = new XNOR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XNOR_ImplicitConversion_WithLongInputs_Complement_ReturnsZero()
    {
        // Arrange
        var a = new Long(0xAAAAAAAAAAAAAAAAL);
        var b = new Long(0x5555555555555555L);
        var expected = new Long(0x0000000000000000L);

        // Act
        Long actual = new XNOR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void XNOR_ImplicitConversion_WithLongInputs_OneZero_ReturnsComplement()
    {
        // Arrange
        var a = new Long(0xAAAAAAAAAAAAAAAAL);
        var b = new Long(0x0000000000000000L);
        var expected = new Long(0x5555555555555555L);

        // Act
        Long actual = new XNOR<Long>(a, b);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // LSB ONLY TESTS (Single bit behavior)
    // ==========================================

    [TestCase(0, 0, 1)]
    [TestCase(0, 1, 0)]
    [TestCase(1, 0, 0)]
    [TestCase(1, 1, 1)]
    public void XNOR_ImplicitConversion_WithByteInputs_ReturnsCorrectLSB(int inputA, int inputB, int expectedOutputInt)
    {
        // Arrange
        var a = new Byte(inputA);
        var b = new Byte(inputB);
        var expected = new Bit(expectedOutputInt);

        // Act
        Byte actualByte = new XNOR<Byte>(a, b);
        Bit actual = new Bit(actualByte.GetBit(0));

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}
