using Turing.Core.Components.Logic;
using Turing.Core.Electricity;

namespace Turing.Tests.Components;

[TestFixture]
internal class BUSTests
{
    // ==========================================
    // BIT TESTS - Exhaustive (2x2x2x2 = 16 cases)
    // ==========================================

    // Format: (inputA, inputB, sel0, sel1, expectedOutputA, expectedOutputB)
    // OutputA = Sel0 == 0 ? InputA : InputB
    // OutputB = Sel1 == 0 ? InputA : InputB

    [TestCase(0, 0, 0, 0, 0, 0)]
    [TestCase(0, 0, 0, 1, 0, 0)]
    [TestCase(0, 0, 1, 0, 0, 0)]
    [TestCase(0, 0, 1, 1, 0, 0)]
    [TestCase(0, 1, 0, 0, 0, 0)] // Both select A (0)
    [TestCase(0, 1, 0, 1, 0, 1)] // OutA=A(0), OutB=B(1)
    [TestCase(0, 1, 1, 0, 1, 0)] // OutA=B(1), OutB=A(0)
    [TestCase(0, 1, 1, 1, 1, 1)] // Both select B (1)
    [TestCase(1, 0, 0, 0, 1, 1)] // Both select A (1)
    [TestCase(1, 0, 0, 1, 1, 0)] // OutA=A(1), OutB=B(0)
    [TestCase(1, 0, 1, 0, 0, 1)] // OutA=B(0), OutB=A(1)
    [TestCase(1, 0, 1, 1, 0, 0)] // Both select B (0)
    [TestCase(1, 1, 0, 0, 1, 1)]
    [TestCase(1, 1, 0, 1, 1, 1)]
    [TestCase(1, 1, 1, 0, 1, 1)]
    [TestCase(1, 1, 1, 1, 1, 1)]
    public void BUS_ImplicitConversion_WithBitInputs_ReturnsCorrectOutput(
        int inputA, int inputB, int sel0, int sel1, int expectedOutputA, int expectedOutputB)
    {
        // Arrange
        var a = new Bit(inputA);
        var b = new Bit(inputB);
        var s0 = new Bit(sel0);
        var s1 = new Bit(sel1);
        var expectedA = new Bit(expectedOutputA);
        var expectedB = new Bit(expectedOutputB);

        // Act
        (Bit OutputA, Bit OutputB) actual = new BUS<Bit>(a, b, s0, s1);

        // Assert
        Assert.That(actual.OutputA, Is.EqualTo(expectedA));
        Assert.That(actual.OutputB, Is.EqualTo(expectedB));
    }

    // ==========================================
    // BYTE TESTS - Exhaustive (256x256x2x2 = 262,144 cases)
    // ==========================================

    [Test]
    public void BUS_ImplicitConversion_WithByteInputs_Exhaustive_ReturnsCorrectOutput()
    {
        for (int a = 0; a < 256; a++)
        {
            for (int b = 0; b < 256; b++)
            {
                for (int sel0 = 0; sel0 <= 1; sel0++)
                {
                    for (int sel1 = 0; sel1 <= 1; sel1++)
                    {
                        // Arrange
                        var inputA = new Byte(a);
                        var inputB = new Byte(b);
                        var s0 = new Bit(sel0);
                        var s1 = new Bit(sel1);

                        // Expected: if sel = 0, output = A; else output = B
                        var expectedA = new Byte(sel0 == 0 ? a : b);
                        var expectedB = new Byte(sel1 == 0 ? a : b);

                        // Act
                        (Byte OutputA, Byte OutputB) actual = new BUS<Byte>(inputA, inputB, s0, s1);

                        // Assert
                        Assert.That(actual.OutputA, Is.EqualTo(expectedA),
                            $"Failed for A={a:X2}, B={b:X2}, Sel0={sel0}, Sel1={sel1}");
                        Assert.That(actual.OutputB, Is.EqualTo(expectedB),
                            $"Failed for A={a:X2}, B={b:X2}, Sel0={sel0}, Sel1={sel1}");
                    }
                }
            }
        }
    }

    [Test]
    public void BUS_ImplicitConversion_WithByteInputs_SpecificCases_ReturnsCorrectOutput()
    {
        // Sel0=0, Sel1=0 -> OutputA=A, OutputB=A
        (Byte OutputA, Byte OutputB) actual1 = new BUS<Byte>(0xAA, 0xCC, new Bit(false), new Bit(false));
        Assert.That(actual1.OutputA, Is.EqualTo(new Byte(0xAA)));
        Assert.That(actual1.OutputB, Is.EqualTo(new Byte(0xAA)));

        // Sel0=0, Sel1=1 -> OutputA=A, OutputB=B
        (Byte OutputA, Byte OutputB) actual2 = new BUS<Byte>(0xAA, 0xCC, new Bit(false), new Bit(true));
        Assert.That(actual2.OutputA, Is.EqualTo(new Byte(0xAA)));
        Assert.That(actual2.OutputB, Is.EqualTo(new Byte(0xCC)));

        // Sel0=1, Sel1=0 -> OutputA=B, OutputB=A
        (Byte OutputA, Byte OutputB) actual3 = new BUS<Byte>(0xAA, 0xCC, new Bit(true), new Bit(false));
        Assert.That(actual3.OutputA, Is.EqualTo(new Byte(0xCC)));
        Assert.That(actual3.OutputB, Is.EqualTo(new Byte(0xAA)));

        // Sel0=1, Sel1=1 -> OutputA=B, OutputB=B
        (Byte OutputA, Byte OutputB) actual4 = new BUS<Byte>(0xAA, 0xCC, new Bit(true), new Bit(true));
        Assert.That(actual4.OutputA, Is.EqualTo(new Byte(0xCC)));
        Assert.That(actual4.OutputB, Is.EqualTo(new Byte(0xCC)));

        // Same values
        (Byte OutputA, Byte OutputB) actual5 = new BUS<Byte>(0xFF, 0xFF, new Bit(true), new Bit(false));
        Assert.That(actual5.OutputA, Is.EqualTo(new Byte(0xFF)));
        Assert.That(actual5.OutputB, Is.EqualTo(new Byte(0xFF)));

        // All zeros
        (Byte OutputA, Byte OutputB) actual6 = new BUS<Byte>(0x00, 0x00, new Bit(true), new Bit(true));
        Assert.That(actual6.OutputA, Is.EqualTo(new Byte(0x00)));
        Assert.That(actual6.OutputB, Is.EqualTo(new Byte(0x00)));
    }

    // ==========================================
    // SHORT TESTS - Selected Cases
    // ==========================================

    [Test]
    public void BUS_ImplicitConversion_WithShortInputs_SelectedCases_ReturnsCorrectOutput()
    {
        (int, int, int, int, int, int)[] testCases = new (int, int, int, int, int, int)[]
        {
            (0x0000, 0x0000, 0, 0, 0x0000, 0x0000),
            (0xAAAA, 0xCCCC, 0, 0, 0xAAAA, 0xAAAA),
            (0xAAAA, 0xCCCC, 0, 1, 0xAAAA, 0xCCCC),
            (0xAAAA, 0xCCCC, 1, 0, 0xCCCC, 0xAAAA),
            (0xAAAA, 0xCCCC, 1, 1, 0xCCCC, 0xCCCC),
            (0xFFFF, 0x0000, 0, 1, 0xFFFF, 0x0000),
            (0xFFFF, 0x0000, 1, 0, 0x0000, 0xFFFF),
            (0x1234, 0x5678, 0, 0, 0x1234, 0x1234),
            (0x1234, 0x5678, 1, 1, 0x5678, 0x5678),
        };

        foreach (var (a, b, sel0, sel1, expectedA, expectedB) in testCases)
        {
            // Arrange
            var inputA = new Short(a);
            var inputB = new Short(b);
            var s0 = new Bit(sel0);
            var s1 = new Bit(sel1);
            var expectedOutputA = new Short(expectedA);
            var expectedOutputB = new Short(expectedB);

            // Act
            (Short OutputA, Short OutputB) actual = new BUS<Short>(inputA, inputB, s0, s1);

            // Assert
            Assert.That(actual.OutputA, Is.EqualTo(expectedOutputA),
                $"Failed for A={a:X4}, B={b:X4}, Sel0={sel0}, Sel1={sel1}");
            Assert.That(actual.OutputB, Is.EqualTo(expectedOutputB),
                $"Failed for A={a:X4}, B={b:X4}, Sel0={sel0}, Sel1={sel1}");
        }
    }

    [Test]
    public void BUS_ImplicitConversion_WithShortInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            int a = random.Next(0x0000, 0xFFFF + 1);
            int b = random.Next(0x0000, 0xFFFF + 1);
            int sel0 = random.Next(0, 2);
            int sel1 = random.Next(0, 2);

            // Arrange
            var inputA = new Short(a);
            var inputB = new Short(b);
            var s0 = new Bit(sel0);
            var s1 = new Bit(sel1);
            var expectedA = new Short(sel0 == 0 ? a : b);
            var expectedB = new Short(sel1 == 0 ? a : b);

            // Act
            (Short OutputA, Short OutputB) actual = new BUS<Short>(inputA, inputB, s0, s1);

            // Assert
            Assert.That(actual.OutputA, Is.EqualTo(expectedA),
                $"Failed for A={a:X4}, B={b:X4}, Sel0={sel0}, Sel1={sel1}");
            Assert.That(actual.OutputB, Is.EqualTo(expectedB),
                $"Failed for A={a:X4}, B={b:X4}, Sel0={sel0}, Sel1={sel1}");
        }
    }

    // ==========================================
    // INT TESTS - Selected Cases
    // ==========================================

    [Test]
    public void BUS_ImplicitConversion_WithIntInputs_SelectedCases_ReturnsCorrectOutput()
    {
        (uint, uint, int, int, uint, uint)[] testCases = new (uint, uint, int, int, uint, uint)[]
        {
            (0x00000000, 0x00000000, 0, 0, 0x00000000, 0x00000000),
            (0xAAAAAAAA, 0xCCCCCCCC, 0, 0, 0xAAAAAAAA, 0xAAAAAAAA),
            (0xAAAAAAAA, 0xCCCCCCCC, 0, 1, 0xAAAAAAAA, 0xCCCCCCCC),
            (0xAAAAAAAA, 0xCCCCCCCC, 1, 0, 0xCCCCCCCC, 0xAAAAAAAA),
            (0xAAAAAAAA, 0xCCCCCCCC, 1, 1, 0xCCCCCCCC, 0xCCCCCCCC),
            (0xFFFFFFFF, 0x00000000, 0, 1, 0xFFFFFFFF, 0x00000000),
            (0xFFFFFFFF, 0x00000000, 1, 0, 0x00000000, 0xFFFFFFFF),
            (0x12345678, 0x87654321, 0, 0, 0x12345678, 0x12345678),
            (0x12345678, 0x87654321, 1, 1, 0x87654321, 0x87654321),
        };

        foreach (var (a, b, sel0, sel1, expectedA, expectedB) in testCases)
        {
            // Arrange
            var inputA = new Int(a);
            var inputB = new Int(b);
            var s0 = new Bit(sel0);
            var s1 = new Bit(sel1);
            var expectedOutputA = new Int(expectedA);
            var expectedOutputB = new Int(expectedB);

            // Act
            (Int OutputA, Int OutputB) actual = new BUS<Int>(inputA, inputB, s0, s1);

            // Assert
            Assert.That(actual.OutputA, Is.EqualTo(expectedOutputA),
                $"Failed for A={a:X8}, B={b:X8}, Sel0={sel0}, Sel1={sel1}");
            Assert.That(actual.OutputB, Is.EqualTo(expectedOutputB),
                $"Failed for A={a:X8}, B={b:X8}, Sel0={sel0}, Sel1={sel1}");
        }
    }

    [Test]
    public void BUS_ImplicitConversion_WithIntInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            int a = random.Next();
            int b = random.Next();
            int sel0 = random.Next(0, 2);
            int sel1 = random.Next(0, 2);

            // Arrange
            var inputA = new Int(a);
            var inputB = new Int(b);
            var s0 = new Bit(sel0);
            var s1 = new Bit(sel1);
            var expectedA = new Int(sel0 == 0 ? a : b);
            var expectedB = new Int(sel1 == 0 ? a : b);

            // Act
            (Int OutputA, Int OutputB) actual = new BUS<Int>(inputA, inputB, s0, s1);

            // Assert
            Assert.That(actual.OutputA, Is.EqualTo(expectedA),
                $"Failed for A={a:X8}, B={b:X8}, Sel0={sel0}, Sel1={sel1}");
            Assert.That(actual.OutputB, Is.EqualTo(expectedB),
                $"Failed for A={a:X8}, B={b:X8}, Sel0={sel0}, Sel1={sel1}");
        }
    }

    // ==========================================
    // LONG TESTS - Selected Cases
    // ==========================================

    [Test]
    public void BUS_ImplicitConversion_WithLongInputs_SelectedCases_ReturnsCorrectOutput()
    {
        (ulong, ulong, int, int, ulong, ulong)[] testCases = new (ulong, ulong, int, int, ulong, ulong)[]
        {
            (0x0000000000000000UL, 0x0000000000000000UL, 0, 0, 0x0000000000000000UL, 0x0000000000000000UL),
            (0xAAAAAAAAAAAAAAAAL, 0xCCCCCCCCCCCCCCCCL, 0, 0, 0xAAAAAAAAAAAAAAAAL, 0xAAAAAAAAAAAAAAAAL),
            (0xAAAAAAAAAAAAAAAAL, 0xCCCCCCCCCCCCCCCCL, 0, 1, 0xAAAAAAAAAAAAAAAAL, 0xCCCCCCCCCCCCCCCCL),
            (0xAAAAAAAAAAAAAAAAL, 0xCCCCCCCCCCCCCCCCL, 1, 0, 0xCCCCCCCCCCCCCCCCL, 0xAAAAAAAAAAAAAAAAL),
            (0xAAAAAAAAAAAAAAAAL, 0xCCCCCCCCCCCCCCCCL, 1, 1, 0xCCCCCCCCCCCCCCCCL, 0xCCCCCCCCCCCCCCCCL),
            (0xFFFFFFFFFFFFFFFFL, 0x0000000000000000L, 0, 1, 0xFFFFFFFFFFFFFFFFL, 0x0000000000000000L),
            (0xFFFFFFFFFFFFFFFFL, 0x0000000000000000L, 1, 0, 0x0000000000000000L, 0xFFFFFFFFFFFFFFFFL),
            (0x123456789ABCDEF0L, 0xFEDCBA987654321FL, 0, 0, 0x123456789ABCDEF0L, 0x123456789ABCDEF0L),
            (0x123456789ABCDEF0L, 0xFEDCBA987654321FL, 1, 1, 0xFEDCBA987654321FL, 0xFEDCBA987654321FL),
        };

        foreach (var (a, b, sel0, sel1, expectedA, expectedB) in testCases)
        {
            // Arrange
            var inputA = new Long(a);
            var inputB = new Long(b);
            var s0 = new Bit(sel0);
            var s1 = new Bit(sel1);
            var expectedOutputA = new Long(expectedA);
            var expectedOutputB = new Long(expectedB);

            // Act
            (Long OutputA, Long OutputB) actual = new BUS<Long>(inputA, inputB, s0, s1);

            // Assert
            Assert.That(actual.OutputA, Is.EqualTo(expectedOutputA),
                $"Failed for A={a:X16}, B={b:X16}, Sel0={sel0}, Sel1={sel1}");
            Assert.That(actual.OutputB, Is.EqualTo(expectedOutputB),
                $"Failed for A={a:X16}, B={b:X16}, Sel0={sel0}, Sel1={sel1}");
        }
    }

    [Test]
    public void BUS_ImplicitConversion_WithLongInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            long a = ((long)random.Next() << 32) | (uint)random.Next();
            long b = ((long)random.Next() << 32) | (uint)random.Next();
            int sel0 = random.Next(0, 2);
            int sel1 = random.Next(0, 2);

            // Arrange
            var inputA = new Long(a);
            var inputB = new Long(b);
            var s0 = new Bit(sel0);
            var s1 = new Bit(sel1);
            var expectedA = new Long(sel0 == 0 ? a : b);
            var expectedB = new Long(sel1 == 0 ? a : b);

            // Act
            (Long OutputA, Long OutputB) actual = new BUS<Long>(inputA, inputB, s0, s1);

            // Assert
            Assert.That(actual.OutputA, Is.EqualTo(expectedA),
                $"Failed for A={a:X16}, B={b:X16}, Sel0={sel0}, Sel1={sel1}");
            Assert.That(actual.OutputB, Is.EqualTo(expectedB),
                $"Failed for A={a:X16}, B={b:X16}, Sel0={sel0}, Sel1={sel1}");
        }
    }

    // ==========================================
    // TYPE PROMOTION TESTS
    // ==========================================

    [Test]
    public void BUS_ImplicitConversion_WithMixedTypes_CompilesAndWorks()
    {
        // Bit to Byte promotion
        var bitA = new Bit(true);
        var bitB = new Bit(false);
        (Byte OutputA, Byte OutputB) actual1 = new BUS<Byte>(bitA, bitB, new Bit(false), new Bit(true));
        Assert.That(actual1.OutputA, Is.EqualTo(new Byte(0x01)));
        Assert.That(actual1.OutputB, Is.EqualTo(new Byte(0x00)));

        // Byte to Short promotion
        var byteA = new Byte(0xAA);
        var byteB = new Byte(0x55);
        (Short OutputA, Short OutputB) actual2 = new BUS<Short>(byteA, byteB, new Bit(true), new Bit(false));
        Assert.That(actual2.OutputA, Is.EqualTo(new Short(0x0055)));
        Assert.That(actual2.OutputB, Is.EqualTo(new Short(0x00AA)));

        // Short to Int promotion
        var shortA = new Short(0xAAAA);
        var shortB = new Short(0x5555);
        (Int OutputA, Int OutputB) actual3 = new BUS<Int>(shortA, shortB, new Bit(false), new Bit(true));
        Assert.That(actual3.OutputA, Is.EqualTo(new Int(0x0000AAAA)));
        Assert.That(actual3.OutputB, Is.EqualTo(new Int(0x00005555)));

        // Int to Long promotion
        var intA = new Int(0xAAAAAAAA);
        var intB = new Int(0x55555555);
        (Long OutputA, Long OutputB) actual4 = new BUS<Long>(intA, intB, new Bit(true), new Bit(true));
        Assert.That(actual4.OutputA, Is.EqualTo(new Long(0x0000000055555555L)));
        Assert.That(actual4.OutputB, Is.EqualTo(new Long(0x0000000055555555L)));
    }
}