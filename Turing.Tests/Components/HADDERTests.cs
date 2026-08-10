using Turing.Core.Components.Logic;

namespace Turing.Tests.Components;

[TestFixture]
internal class HADDERTests
{
    // ==========================================
    // BIT TESTS - Exhaustive (2x2 = 4 cases)
    // ==========================================

    [TestCase(0, 0, 0, 0)]
    [TestCase(0, 1, 1, 0)]
    [TestCase(1, 0, 1, 0)]
    [TestCase(1, 1, 0, 1)]
    public void HADDER_ImplicitConversion_WithBitInputs_ReturnsCorrectOutput(
        int inputA, int inputB, int expectedSum, int expectedCarry)
    {
        // Arrange
        var a = new Bit(inputA);
        var b = new Bit(inputB);
        var expectedSumBit = new Bit(expectedSum);
        var expectedCarryBit = new Bit(expectedCarry);

        // Act
        (Bit Sum, Bit Carry) actual = new HADDER<Bit>(a, b);

        // Assert
        Assert.That(actual.Sum, Is.EqualTo(expectedSumBit));
        Assert.That(actual.Carry, Is.EqualTo(expectedCarryBit));
    }

    // ==========================================
    // BYTE TESTS - Exhaustive (256x256 = 65,536 cases)
    // ==========================================

    [Test]
    public void HADDER_ImplicitConversion_WithByteInputs_Exhaustive_ReturnsCorrectOutput()
    {
        for (int a = 0; a < 256; a++)
        {
            for (int b = 0; b < 256; b++)
            {
                // Arrange
                var inputA = new Byte(a);
                var inputB = new Byte(b);
                var expectedSum = new Byte(a ^ b);
                var expectedCarry = new Byte(a & b);

                // Act
                (Byte Sum, Byte Carry) actual = new HADDER<Byte>(inputA, inputB);

                // Assert
                Assert.That(actual.Sum, Is.EqualTo(expectedSum), $"Failed for A={a:X2}, B={b:X2}");
                Assert.That(actual.Carry, Is.EqualTo(expectedCarry), $"Failed for A={a:X2}, B={b:X2}");
            }
        }
    }

    [Test]
    public void HADDER_ImplicitConversion_WithByteInputs_SpecificCases_ReturnsCorrectOutput()
    {
        // All zeros
        (Byte Sum, Byte Carry) actual1 = new HADDER<Byte>(0x00, 0x00);
        Assert.That(actual1.Sum, Is.EqualTo(new Byte(0x00)));
        Assert.That(actual1.Carry, Is.EqualTo(new Byte(0x00)));

        // All ones
        (Byte Sum, Byte Carry) actual2 = new HADDER<Byte>(0xFF, 0xFF);
        Assert.That(actual2.Sum, Is.EqualTo(new Byte(0x00)));
        Assert.That(actual2.Carry, Is.EqualTo(new Byte(0xFF)));

        // Alternating
        (Byte Sum, Byte Carry) actual3 = new HADDER<Byte>(0xAA, 0x55);
        Assert.That(actual3.Sum, Is.EqualTo(new Byte(0xFF)));
        Assert.That(actual3.Carry, Is.EqualTo(new Byte(0x00)));

        // Same value
        (Byte Sum, Byte Carry) actual4 = new HADDER<Byte>(0xAA, 0xAA);
        Assert.That(actual4.Sum, Is.EqualTo(new Byte(0x00)));
        Assert.That(actual4.Carry, Is.EqualTo(new Byte(0xAA)));

        // One zero
        (Byte Sum, Byte Carry) actual5 = new HADDER<Byte>(0xAA, 0x00);
        Assert.That(actual5.Sum, Is.EqualTo(new Byte(0xAA)));
        Assert.That(actual5.Carry, Is.EqualTo(new Byte(0x00)));

        // A=2, B=2
        (Byte Sum, Byte Carry) actual6 = new HADDER<Byte>(0x02, 0x02);
        Assert.That(actual6.Sum, Is.EqualTo(new Byte(0x00)));
        Assert.That(actual6.Carry, Is.EqualTo(new Byte(0x02)));
    }

    // ==========================================
    // SHORT TESTS - Selected Cases with Dynamic Calculation
    // ==========================================

    [Test]
    public void HADDER_ImplicitConversion_WithShortInputs_SelectedCases_ReturnsCorrectOutput()
    {
        (int, int)[] testCases = new (int, int)[]
        {
            (0x0000, 0x0000),
            (0xFFFF, 0xFFFF),
            (0xAAAA, 0x5555),
            (0xAAAA, 0xAAAA),
            (0xAAAA, 0x0000),
            (0x1234, 0x5678),
            (0xFFFF, 0x0000),
            (0x8000, 0x8000),
        };

        foreach (var (a, b) in testCases)
        {
            // Arrange
            var inputA = new Short(a);
            var inputB = new Short(b);
            var expectedSum = new Short(a ^ b);
            var expectedCarry = new Short(a & b);

            // Act
            (Short Sum, Short Carry) actual = new HADDER<Short>(inputA, inputB);

            // Assert
            Assert.That(actual.Sum, Is.EqualTo(expectedSum),
                $"Failed for A={a:X4}, B={b:X4}");
            Assert.That(actual.Carry, Is.EqualTo(expectedCarry),
                $"Failed for A={a:X4}, B={b:X4}");
        }
    }

    [Test]
    public void HADDER_ImplicitConversion_WithShortInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            int a = random.Next(0x0000, 0xFFFF + 1);
            int b = random.Next(0x0000, 0xFFFF + 1);

            // Arrange
            var inputA = new Short(a);
            var inputB = new Short(b);
            var expectedSum = new Short(a ^ b);
            var expectedCarry = new Short(a & b);

            // Act
            (Short Sum, Short Carry) actual = new HADDER<Short>(inputA, inputB);

            // Assert
            Assert.That(actual.Sum, Is.EqualTo(expectedSum),
                $"Failed for A={a:X4}, B={b:X4}");
            Assert.That(actual.Carry, Is.EqualTo(expectedCarry),
                $"Failed for A={a:X4}, B={b:X4}");
        }
    }

    // ==========================================
    // INT TESTS - Selected Cases with Dynamic Calculation
    // ==========================================

    [Test]
    public void HADDER_ImplicitConversion_WithIntInputs_SelectedCases_ReturnsCorrectOutput()
    {
        (uint, uint)[] testCases =
        [
            (0x00000000, 0x00000000),
            (0xFFFFFFFF, 0xFFFFFFFF),
            (0xAAAAAAAA, 0x55555555),
            (0xAAAAAAAA, 0xAAAAAAAA),
            (0xAAAAAAAA, 0x00000000),
            (0x12345678, 0x87654321),
            (0xFFFFFFFF, 0x00000000),
            (0x80000000, 0x80000000),
        ];

        foreach (var (a, b) in testCases)
        {
            // Arrange
            var inputA = new Int(a);
            var inputB = new Int(b);
            var expectedSum = new Int(a ^ b);
            var expectedCarry = new Int(a & b);

            // Act
            (Int Sum, Int Carry) actual = new HADDER<Int>(inputA, inputB);

            // Assert
            Assert.That(actual.Sum, Is.EqualTo(expectedSum),
                $"Failed for A={a:X8}, B={b:X8}");
            Assert.That(actual.Carry, Is.EqualTo(expectedCarry),
                $"Failed for A={a:X8}, B={b:X8}");
        }
    }

    [Test]
    public void HADDER_ImplicitConversion_WithIntInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            int a = random.Next();
            int b = random.Next();

            // Arrange
            var inputA = new Int(a);
            var inputB = new Int(b);
            var expectedSum = new Int(a ^ b);
            var expectedCarry = new Int(a & b);

            // Act
            (Int Sum, Int Carry) actual = new HADDER<Int>(inputA, inputB);

            // Assert
            Assert.That(actual.Sum, Is.EqualTo(expectedSum),
                $"Failed for A={a:X8}, B={b:X8}");
            Assert.That(actual.Carry, Is.EqualTo(expectedCarry),
                $"Failed for A={a:X8}, B={b:X8}");
        }
    }

    // ==========================================
    // LONG TESTS - Selected Cases with Dynamic Calculation
    // ==========================================

    [Test]
    public void HADDER_ImplicitConversion_WithLongInputs_SelectedCases_ReturnsCorrectOutput()
    {
        (ulong, ulong)[] testCases =
        [
            (0x0000000000000000L, 0x0000000000000000L),
            (0xFFFFFFFFFFFFFFFFL, 0xFFFFFFFFFFFFFFFFL),
            (0xAAAAAAAAAAAAAAAAL, 0x5555555555555555L),
            (0xAAAAAAAAAAAAAAAAL, 0xAAAAAAAAAAAAAAAAL),
            (0xAAAAAAAAAAAAAAAAL, 0x0000000000000000L),
            (0x123456789ABCDEF0L, 0xFEDCBA987654321FL),
            (0xFFFFFFFFFFFFFFFFL, 0x0000000000000000L),
            (0x8000000000000000L, 0x8000000000000000L),
        ];

        foreach (var (a, b) in testCases)
        {
            // Arrange
            var inputA = new Long(a);
            var inputB = new Long(b);
            var expectedSum = new Long(a ^ b);
            var expectedCarry = new Long(a & b);

            // Act
            (Long Sum, Long Carry) actual = new HADDER<Long>(inputA, inputB);

            // Assert
            Assert.That(actual.Sum, Is.EqualTo(expectedSum),
                $"Failed for A={a:X16}, B={b:X16}");
            Assert.That(actual.Carry, Is.EqualTo(expectedCarry),
                $"Failed for A={a:X16}, B={b:X16}");
        }
    }

    [Test]
    public void HADDER_ImplicitConversion_WithLongInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            long a = ((long)random.Next() << 32) | (uint)random.Next();
            long b = ((long)random.Next() << 32) | (uint)random.Next();

            // Arrange
            var inputA = new Long(a);
            var inputB = new Long(b);
            var expectedSum = new Long(a ^ b);
            var expectedCarry = new Long(a & b);

            // Act
            (Long Sum, Long Carry) actual = new HADDER<Long>(inputA, inputB);

            // Assert
            Assert.That(actual.Sum, Is.EqualTo(expectedSum),
                $"Failed for A={a:X16}, B={b:X16}");
            Assert.That(actual.Carry, Is.EqualTo(expectedCarry),
                $"Failed for A={a:X16}, B={b:X16}");
        }
    }

    // ==========================================
    // TYPE PROMOTION TESTS
    // ==========================================

    [Test]
    public void HADDER_ImplicitConversion_WithMixedTypes_CompilesAndWorks()
    {
        // Bit to Byte promotion
        var bitA = new Bit(true);
        var bitB = new Bit(false);
        (Byte Sum, Byte Carry) actual1 = new HADDER<Byte>(bitA, bitB);
        Assert.That(actual1.Sum, Is.EqualTo(new Byte(0x01)));
        Assert.That(actual1.Carry, Is.EqualTo(new Byte(0x00)));

        // Byte to Short promotion
        var byteA = new Byte(0xAA);
        var byteB = new Byte(0x55);
        (Short Sum, Short Carry) actual2 = new HADDER<Short>(byteA, byteB);
        Assert.That(actual2.Sum, Is.EqualTo(new Short(0x00FF)));
        Assert.That(actual2.Carry, Is.EqualTo(new Short(0x0000)));

        // Short to Int promotion
        var shortA = new Short(0xAAAA);
        var shortB = new Short(0x5555);
        (Int Sum, Int Carry) actual3 = new HADDER<Int>(shortA, shortB);
        Assert.That(actual3.Sum, Is.EqualTo(new Int(0x0000FFFF)));
        Assert.That(actual3.Carry, Is.EqualTo(new Int(0x00000000)));

        // Int to Long promotion
        var intA = new Int(0xAAAAAAAA);
        var intB = new Int(0x55555555);
        (Long Sum, Long Carry) actual4 = new HADDER<Long>(intA, intB);
        Assert.That(actual4.Sum, Is.EqualTo(new Long(0x00000000FFFFFFFFL)));
        Assert.That(actual4.Carry, Is.EqualTo(new Long(0x0000000000000000L)));
    }
}