using Turing.Core.Components.Logic;
using Turing.Core.Electricity;

namespace Turing.Tests.Components;

[TestFixture]
internal class MUXTests
{
    [TestCase(0, 0, 0, 0)]
    [TestCase(0, 0, 1, 0)]
    [TestCase(0, 1, 0, 0)]
    [TestCase(0, 1, 1, 1)]
    [TestCase(1, 0, 0, 1)]
    [TestCase(1, 0, 1, 0)]
    [TestCase(1, 1, 0, 1)]
    [TestCase(1, 1, 1, 1)]
    public void MUX_ImplicitConversion_WithBitInputs_ReturnsCorrectOutput(
        int inputA, int inputB, int inputSel, int expectedOutput)
    {
        // Arrange
        var a = new Bit(inputA);
        var b = new Bit(inputB);
        var sel = new Bit(inputSel);
        var expected = new Bit(expectedOutput);

        // Act
        Bit actual = new MUX<Bit>(a, b, sel);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    // ==========================================
    // BYTE TESTS - Exhaustive (256x256x2 = 131,072 cases)
    // ==========================================

    [Test]
    public void MUX_ImplicitConversion_WithByteInputs_Exhaustive_ReturnsCorrectOutput()
    {
        for (int a = 0; a < 256; a++)
        {
            for (int b = 0; b < 256; b++)
            {
                for (int sel = 0; sel <= 1; sel++)
                {
                    // Arrange
                    var inputA = new Byte(a);
                    var inputB = new Byte(b);
                    var select = new Bit(sel);

                    // Expected: if sel = 0, output = a; else output = b
                    var expected = new Byte(sel == 0 ? a : b);

                    // Act
                    Byte actual = new MUX<Byte>(inputA, inputB, select);

                    // Assert
                    Assert.That(actual, Is.EqualTo(expected),
                        $"Failed for A={a:X2}, B={b:X2}, Sel={sel}");
                }
            }
        }
    }

    [Test]
    public void MUX_ImplicitConversion_WithByteInputs_SpecificCases_ReturnsCorrectOutput()
    {
        // Sel = 0 -> Output = A
        Byte actual1 = new MUX<Byte>(0xAA, 0xCC, new Bit(false));
        Assert.That(actual1, Is.EqualTo(new Byte(0xAA)));

        // Sel = 1 -> Output = B
        Byte actual2 = new MUX<Byte>(0xAA, 0xCC, new Bit(true));
        Assert.That(actual2, Is.EqualTo(new Byte(0xCC)));

        // Sel = 0 with same values
        Byte actual3 = new MUX<Byte>(0xFF, 0xFF, new Bit(false));
        Assert.That(actual3, Is.EqualTo(new Byte(0xFF)));

        // Sel = 1 with same values
        Byte actual4 = new MUX<Byte>(0xFF, 0xFF, new Bit(true));
        Assert.That(actual4, Is.EqualTo(new Byte(0xFF)));

        // Sel = 0 with zeros
        Byte actual5 = new MUX<Byte>(0x00, 0x00, new Bit(false));
        Assert.That(actual5, Is.EqualTo(new Byte(0x00)));

        // Sel = 1 with zeros
        Byte actual6 = new MUX<Byte>(0x00, 0x00, new Bit(true));
        Assert.That(actual6, Is.EqualTo(new Byte(0x00)));

        // A=0x02, B=0x02, Sel=1 (the failing case)
        Byte actual7 = new MUX<Byte>(0x02, 0x02, new Bit(true));
        Assert.That(actual7, Is.EqualTo(new Byte(0x02)));

        // A=0x02, B=0x04, Sel=1
        Byte actual8 = new MUX<Byte>(0x02, 0x04, new Bit(true));
        Assert.That(actual8, Is.EqualTo(new Byte(0x04)));
    }

    // ==========================================
    // SHORT TESTS - Selected Cases
    // ==========================================

    [Test]
    public void MUX_ImplicitConversion_WithShortInputs_SelectedCases_ReturnsCorrectOutput()
    {
        (int, int, int, int)[] testCases = new (int, int, int, int)[]
        {
            (0x0000, 0x0000, 0, 0x0000),
            (0x0000, 0x0000, 1, 0x0000),
            (0xAAAA, 0xCCCC, 0, 0xAAAA),
            (0xAAAA, 0xCCCC, 1, 0xCCCC),
            (0xFFFF, 0x0000, 0, 0xFFFF),
            (0xFFFF, 0x0000, 1, 0x0000),
            (0x1234, 0x5678, 0, 0x1234),
            (0x1234, 0x5678, 1, 0x5678),
            (0x8000, 0x8000, 0, 0x8000),
            (0x8000, 0x8000, 1, 0x8000),
            (0x0002, 0x0002, 1, 0x0002),
            (0x0002, 0x0004, 1, 0x0004),
        };

        foreach (var (a, b, sel, expected) in testCases)
        {
            // Arrange
            var inputA = new Short(a);
            var inputB = new Short(b);
            var select = new Bit(sel);
            var expectedOutput = new Short(expected);

            // Act
            Short actual = new MUX<Short>(inputA, inputB, select);

            // Assert
            Assert.That(actual, Is.EqualTo(expectedOutput),
                $"Failed for A={a:X4}, B={b:X4}, Sel={sel}");
        }
    }

    [Test]
    public void MUX_ImplicitConversion_WithShortInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            int a = random.Next(0x0000, 0xFFFF + 1);
            int b = random.Next(0x0000, 0xFFFF + 1);
            int sel = random.Next(0, 2);

            // Arrange
            var inputA = new Short(a);
            var inputB = new Short(b);
            var select = new Bit(sel);
            var expected = new Short(sel == 0 ? a : b);

            // Act
            Short actual = new MUX<Short>(inputA, inputB, select);

            // Assert
            Assert.That(actual, Is.EqualTo(expected),
                $"Failed for A={a:X4}, B={b:X4}, Sel={sel}");
        }
    }

    // ==========================================
    // INT TESTS - Selected Cases
    // ==========================================

    [Test]
    public void MUX_ImplicitConversion_WithIntInputs_SelectedCases_ReturnsCorrectOutput()
    {
        (uint, uint, int, uint)[] testCases = new (uint, uint, int, uint)[]
        {
            (0x00000000, 0x00000000, 0, 0x00000000),
            (0x00000000, 0x00000000, 1, 0x00000000),
            (0xAAAAAAAA, 0xCCCCCCCC, 0, 0xAAAAAAAA),
            (0xAAAAAAAA, 0xCCCCCCCC, 1, 0xCCCCCCCC),
            (0xFFFFFFFF, 0x00000000, 0, 0xFFFFFFFF),
            (0xFFFFFFFF, 0x00000000, 1, 0x00000000),
            (0x12345678, 0x87654321, 0, 0x12345678),
            (0x12345678, 0x87654321, 1, 0x87654321),
            (0x80000000, 0x80000000, 0, 0x80000000),
            (0x80000000, 0x80000000, 1, 0x80000000),
            (0x00000002, 0x00000002, 1, 0x00000002),
            (0x00000002, 0x00000004, 1, 0x00000004),
        };

        foreach (var (a, b, sel, expected) in testCases)
        {
            // Arrange
            var inputA = new Int(a);
            var inputB = new Int(b);
            var select = new Bit(sel);
            var expectedOutput = new Int(expected);

            // Act
            Int actual = new MUX<Int>(inputA, inputB, select);

            // Assert
            Assert.That(actual, Is.EqualTo(expectedOutput),
                $"Failed for A={a:X8}, B={b:X8}, Sel={sel}");
        }
    }

    [Test]
    public void MUX_ImplicitConversion_WithIntInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            int a = random.Next();
            int b = random.Next();
            int sel = random.Next(0, 2);

            // Arrange
            var inputA = new Int(a);
            var inputB = new Int(b);
            var select = new Bit(sel);
            var expected = new Int(sel == 0 ? a : b);

            // Act
            Int actual = new MUX<Int>(inputA, inputB, select);

            // Assert
            Assert.That(actual, Is.EqualTo(expected),
                $"Failed for A={a:X8}, B={b:X8}, Sel={sel}");
        }
    }

    // ==========================================
    // LONG TESTS - Selected Cases
    // ==========================================

    [Test]
    public void MUX_ImplicitConversion_WithLongInputs_SelectedCases_ReturnsCorrectOutput()
    {
        (ulong, ulong, int, ulong)[] testCases = new (ulong, ulong, int, ulong)[]
        {
            (0x0000000000000000UL, 0x0000000000000000UL, 0, 0x0000000000000000UL),
            (0x0000000000000000UL, 0x0000000000000000UL, 1, 0x0000000000000000UL),
            (0xAAAAAAAAAAAAAAAAL, 0xCCCCCCCCCCCCCCCCL, 0, 0xAAAAAAAAAAAAAAAAL),
            (0xAAAAAAAAAAAAAAAAL, 0xCCCCCCCCCCCCCCCCL, 1, 0xCCCCCCCCCCCCCCCCL),
            (0xFFFFFFFFFFFFFFFFL, 0x0000000000000000L, 0, 0xFFFFFFFFFFFFFFFFL),
            (0xFFFFFFFFFFFFFFFFL, 0x0000000000000000L, 1, 0x0000000000000000L),
            (0x123456789ABCDEF0L, 0xFEDCBA987654321FL, 0, 0x123456789ABCDEF0L),
            (0x123456789ABCDEF0L, 0xFEDCBA987654321FL, 1, 0xFEDCBA987654321FL),
            (0x8000000000000000L, 0x8000000000000000L, 0, 0x8000000000000000L),
            (0x8000000000000000L, 0x8000000000000000L, 1, 0x8000000000000000L),
            (0x0000000000000002L, 0x0000000000000002L, 1, 0x0000000000000002L),
            (0x0000000000000002L, 0x0000000000000004L, 1, 0x0000000000000004L),
        };

        foreach (var (a, b, sel, expected) in testCases)
        {
            // Arrange
            var inputA = new Long(a);
            var inputB = new Long(b);
            var select = new Bit(sel);
            var expectedOutput = new Long(expected);

            // Act
            Long actual = new MUX<Long>(inputA, inputB, select);

            // Assert
            Assert.That(actual, Is.EqualTo(expectedOutput),
                $"Failed for A={a:X16}, B={b:X16}, Sel={sel}");
        }
    }

    [Test]
    public void MUX_ImplicitConversion_WithLongInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            long a = ((long)random.Next() << 32) | (uint)random.Next();
            long b = ((long)random.Next() << 32) | (uint)random.Next();
            int sel = random.Next(0, 2);

            // Arrange
            var inputA = new Long(a);
            var inputB = new Long(b);
            var select = new Bit(sel);
            var expected = new Long(sel == 0 ? a : b);

            // Act
            Long actual = new MUX<Long>(inputA, inputB, select);

            // Assert
            Assert.That(actual, Is.EqualTo(expected),
                $"Failed for A={a:X16}, B={b:X16}, Sel={sel}");
        }
    }

    // ==========================================
    // TYPE PROMOTION TESTS
    // ==========================================

    [Test]
    public void MUX_ImplicitConversion_WithMixedTypes_CompilesAndWorks()
    {
        // Bit to Byte promotion
        var bitA = new Bit(true);
        var bitB = new Bit(false);
        Byte actual1 = new MUX<Byte>((int)bitA, (int)bitB, new Bit(false));
        Assert.That(actual1, Is.EqualTo(new Byte(0x01)));

        Byte actual2 = new MUX<Byte>((int)bitA, (int)bitB, new Bit(true));
        Assert.That(actual2, Is.EqualTo(new Byte(0x00)));

        // Byte to Short promotion
        var byteA = new Byte(0xAA);
        var byteB = new Byte(0x55);
        Short actual3 = new MUX<Short>(byteA, byteB, new Bit(false));
        Assert.That(actual3, Is.EqualTo(new Short(0x00AA)));

        Short actual4 = new MUX<Short>(byteA, byteB, new Bit(true));
        Assert.That(actual4, Is.EqualTo(new Short(0x0055)));

        // Short to Int promotion
        var shortA = new Short(0xAAAA);
        var shortB = new Short(0x5555);
        Int actual5 = new MUX<Int>(shortA, shortB, new Bit(false));
        Assert.That(actual5, Is.EqualTo(new Int(0x0000AAAA)));

        Int actual6 = new MUX<Int>(shortA, shortB, new Bit(true));
        Assert.That(actual6, Is.EqualTo(new Int(0x00005555)));

        // Int to Long promotion
        var intA = new Int(0xAAAAAAAA);
        var intB = new Int(0x55555555);
        Long actual7 = new MUX<Long>(intA, intB, new Bit(false));
        Assert.That(actual7, Is.EqualTo(new Long(0x00000000AAAAAAAA)));

        Long actual8 = new MUX<Long>(intA, intB, new Bit(true));
        Assert.That(actual8, Is.EqualTo(new Long(0x0000000055555555)));
    }
}