using Turing.Core.Gates;

namespace Turing.Tests.Gates;

[TestFixture]
internal class NOTTests
{
    // ==========================================
    // BIT TESTS
    // ==========================================

    [TestCase(0, 1)]
    [TestCase(1, 0)]
    public void NOT_ImplicitConversion_WithBitInputs_ReturnsCorrectOutput(int inputInt, int expectedOutputInt)
    {
        // Arrange
        var input = new Bit(inputInt);
        var expected = new Bit(expectedOutputInt);

        // Act
        Bit actual = new NOT<Bit>(input);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // BYTE TESTS
    // ==========================================

    [Test]
    public void NOT_ImplicitConversion_WithByteInputs_ReturnsBitwiseNOT()
    {
        // Arrange
        var input = new Byte(0xAA); // 10101010
        var expected = new Byte(0x55); // 01010101

        // Act
        Byte actual = new NOT<Byte>(input);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOT_ImplicitConversion_WithByteInputs_AllOnes_ReturnsZero()
    {
        // Arrange
        var input = new Byte(0xFF);
        var expected = new Byte(0x00);

        // Act
        Byte actual = new NOT<Byte>(input);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOT_ImplicitConversion_WithByteInputs_AllZeros_ReturnsAllOnes()
    {
        // Arrange
        var input = new Byte(0x00);
        var expected = new Byte(0xFF);

        // Act
        Byte actual = new NOT<Byte>(input);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOT_ImplicitConversion_WithByteInputs_Alternating_ReturnsInverted()
    {
        // Arrange
        var input = new Byte(0b10101010);
        var expected = new Byte(0b01010101);

        // Act
        Byte actual = new NOT<Byte>(input);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // SHORT TESTS
    // ==========================================

    [Test]
    public void NOT_ImplicitConversion_WithShortInputs_ReturnsBitwiseNOT()
    {
        // Arrange
        var input = new Short(0xAAAA);
        var expected = new Short(0x5555);

        // Act
        Short actual = new NOT<Short>(input);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOT_ImplicitConversion_WithShortInputs_AllOnes_ReturnsZero()
    {
        // Arrange
        var input = new Short(0xFFFF);
        var expected = new Short(0x0000);

        // Act
        Short actual = new NOT<Short>(input);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOT_ImplicitConversion_WithShortInputs_AllZeros_ReturnsAllOnes()
    {
        // Arrange
        var input = new Short(0x0000);
        var expected = new Short(0xFFFF);

        // Act
        Short actual = new NOT<Short>(input);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // INT TESTS
    // ==========================================

    [Test]
    public void NOT_ImplicitConversion_WithIntInputs_ReturnsBitwiseNOT()
    {
        // Arrange
        var input = new Int(0xAAAAAAAA);
        var expected = new Int(0x55555555);

        // Act
        Int actual = new NOT<Int>(input);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOT_ImplicitConversion_WithIntInputs_AllOnes_ReturnsZero()
    {
        // Arrange
        var input = new Int(0xFFFFFFFF);
        var expected = new Int(0x00000000);

        // Act
        Int actual = new NOT<Int>(input);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOT_ImplicitConversion_WithIntInputs_AllZeros_ReturnsAllOnes()
    {
        // Arrange
        var input = new Int(0x00000000);
        var expected = new Int(0xFFFFFFFF);

        // Act
        Int actual = new NOT<Int>(input);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // LONG TESTS
    // ==========================================

    [Test]
    public void NOT_ImplicitConversion_WithLongInputs_ReturnsBitwiseNOT()
    {
        // Arrange
        var input = new Long(0xAAAAAAAAAAAAAAAAL);
        var expected = new Long(0x5555555555555555L);

        // Act
        Long actual = new NOT<Long>(input);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOT_ImplicitConversion_WithLongInputs_AllOnes_ReturnsZero()
    {
        // Arrange
        var input = new Long(0xFFFFFFFFFFFFFFFFL);
        var expected = new Long(0x0000000000000000L);

        // Act
        Long actual = new NOT<Long>(input);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void NOT_ImplicitConversion_WithLongInputs_AllZeros_ReturnsAllOnes()
    {
        // Arrange
        var input = new Long(0x0000000000000000L);
        var expected = new Long(0xFFFFFFFFFFFFFFFFL);

        // Act
        Long actual = new NOT<Long>(input);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // LSB ONLY TESTS (Single bit behavior)
    // ==========================================

    [TestCase(0, 1)]
    [TestCase(1, 0)]
    public void NOT_ImplicitConversion_WithByteInputs_ReturnsCorrectLSB(int inputInt, int expectedOutputInt)
    {
        // Arrange
        var input = new Byte(inputInt);
        var expected = new Bit(expectedOutputInt);

        // Act
        Byte actualByte = new NOT<Byte>(input);
        Bit actual = new Bit((bool)actualByte.GetBit(0));

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}
