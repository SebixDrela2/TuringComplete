using Turing.Core.Components.Logic;

namespace Turing.Tests.Components;

[TestFixture]
internal class ADDERTests
{
    // ==========================================
    // BIT TESTS - Exhaustive (2x2x2 = 8 cases)
    // ==========================================

    [TestCase(0, 0, 0, 0, 0)]
    [TestCase(0, 0, 1, 1, 0)]
    [TestCase(0, 1, 0, 1, 0)]
    [TestCase(0, 1, 1, 0, 1)]
    [TestCase(1, 0, 0, 1, 0)]
    [TestCase(1, 0, 1, 0, 1)]
    [TestCase(1, 1, 0, 0, 1)]
    [TestCase(1, 1, 1, 1, 1)]
    public void ADDER_ImplicitConversion_WithBitInputs_ReturnsCorrectOutput(
        int inputA, int inputB, int inputCin, int expectedSum, int expectedCarry)
    {
        // Arrange
        var a = new Bit(inputA);
        var b = new Bit(inputB);
        var cin = new Bit(inputCin);
        var expectedSumBit = new Bit(expectedSum);
        var expectedCarryBit = new Bit(expectedCarry);

        // Act
        (Bit Sum, Bit Carry) actual = new ADDER<Bit>(a, b, cin);

        // Assert
        Assert.That(actual.Sum, Is.EqualTo(expectedSumBit));
        Assert.That(actual.Carry, Is.EqualTo(expectedCarryBit));
    }

    // ==========================================
    // BYTE TESTS - Exhaustive (256x256x2 = 131,072 cases)
    // ==========================================

    [Test]
    public void ADDER_ImplicitConversion_WithByteInputs_Exhaustive_ReturnsCorrectOutput()
    {
        for (int a = 0; a < 256; a++)
        {
            for (int b = 0; b < 256; b++)
            {
                for (int cin = 0; cin <= 1; cin++)
                {
                    // Calculate expected: Sum = (a + b + cin) & 0xFF, Carry = (a + b + cin) >> 8
                    int total = a + b + cin;
                    var expectedSum = new Byte(total & 0xFF);
                    var expectedCarry = new Bit((total >> 8) > 0);

                    // Arrange
                    var inputA = new Byte(a);
                    var inputB = new Byte(b);
                    var inputCin = new Bit(cin);

                    // Act
                    (Byte Sum, Bit Carry) actual = new ADDER<Byte>(inputA, inputB, inputCin);

                    // Assert
                    Assert.That(actual.Sum, Is.EqualTo(expectedSum),
                        $"Failed for A={a:X2}, B={b:X2}, Cin={cin}");
                    Assert.That(actual.Carry, Is.EqualTo(expectedCarry),
                        $"Failed for A={a:X2}, B={b:X2}, Cin={cin}");
                }
            }
        }
    }

    [Test]
    public void ADDER_ImplicitConversion_WithByteInputs_SpecificCases_ReturnsCorrectOutput()
    {
        // All zeros
        (Byte Sum, Bit Carry) actual1 = new ADDER<Byte>(0x00, 0x00, 0x00);
        Assert.That(actual1.Sum, Is.EqualTo(new Byte(0x00)));
        Assert.That(actual1.Carry, Is.EqualTo(new Bit(0x00)));

        // Max values with carry
        (Byte Sum, Bit Carry) actual2 = new ADDER<Byte>(0xFF, 0xFF, 0x01);
        Assert.That(actual2.Sum, Is.EqualTo(new Byte(0xFF))); // 255 + 255 + 1 = 511, low byte = 255
        Assert.That(actual2.Carry, Is.EqualTo(new Bit(0x01))); // carry = 1

        // Max values without carry
        (Byte Sum, Bit Carry) actual3 = new ADDER<Byte>(0xFF, 0xFF, 0x00);
        Assert.That(actual3.Sum, Is.EqualTo(new Byte(0xFE))); // 255 + 255 = 510, low byte = 254
        Assert.That(actual3.Carry, Is.EqualTo(new Bit(0x01))); // carry = 1

        // Half values
        (Byte Sum, Bit Carry) actual4 = new ADDER<Byte>(0x80, 0x80, 0x00);
        Assert.That(actual4.Sum, Is.EqualTo(new Byte(0x00))); // 128 + 128 = 256, low byte = 0
        Assert.That(actual4.Carry, Is.EqualTo(new Bit(0x01))); // carry = 1

        // Random values
        (Byte Sum, Bit Carry) actual5 = new ADDER<Byte>(0xAA, 0x55, 0x01);
        Assert.That(actual5.Sum, Is.EqualTo(new Byte(0x00))); // 170 + 85 + 1 = 256, low byte = 0
        Assert.That(actual5.Carry, Is.EqualTo(new Bit(0x01))); // carry = 1

        // With cin = 1 adding 1
        (Byte Sum, Bit Carry) actual6 = new ADDER<Byte>(0x00, 0x00, 0x01);
        Assert.That(actual6.Sum, Is.EqualTo(new Byte(0x01)));
        Assert.That(actual6.Carry, Is.EqualTo(new Bit(0x00)));
    }

    // ==========================================
    // SHORT TESTS - Selected Cases with Dynamic Calculation
    // ==========================================

    [Test]
    public void ADDER_ImplicitConversion_WithShortInputs_SelectedCases_ReturnsCorrectOutput()
    {
        (int, int, int)[] testCases = new (int, int, int)[]
        {
            (0x0000, 0x0000, 0x0000),
            (0xFFFF, 0xFFFF, 0x0000),
            (0xFFFF, 0xFFFF, 0x0001),
            (0xAAAA, 0x5555, 0x0000),
            (0xAAAA, 0xAAAA, 0x0000),
            (0xAAAA, 0x0000, 0x0001),
            (0x1234, 0x5678, 0x0000),
            (0x1234, 0x5678, 0x0001),
            (0xFFFF, 0x0000, 0x0001),
            (0x8000, 0x8000, 0x0000),
            (0x8000, 0x8000, 0x0001),
        };

        foreach (var (a, b, cin) in testCases)
        {
            // Calculate expected
            int total = a + b + cin;
            var expectedSum = new Short(total & 0xFFFF);
            var expectedCarry = new Short((total >> 16) & 0xFFFF);

            // Arrange
            var inputA = new Short(a);
            var inputB = new Short(b);
            var inputCin = new Bit(cin);

            // Act
            (Short Sum, Bit Carry) actual = new ADDER<Short>(inputA, inputB, inputCin);

            // Assert
            Assert.That(actual.Sum, Is.EqualTo(expectedSum),
                $"Failed for A={a:X4}, B={b:X4}, Cin={cin}");
            Assert.That(actual.Carry, Is.EqualTo((Bit)(int)expectedCarry),
                $"Failed for A={a:X4}, B={b:X4}, Cin={cin}");
        }
    }

    [Test]
    public void ADDER_ImplicitConversion_WithShortInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            int a = random.Next(0x0000, 0xFFFF + 1);
            int b = random.Next(0x0000, 0xFFFF + 1);
            int cin = random.Next(0, 2);

            // Calculate expected
            int total = a + b + cin;
            var expectedSum = new Short(total & 0xFFFF);
            var expectedCarry = new Short((total >> 16) & 0xFFFF);

            // Arrange
            var inputA = new Short(a);
            var inputB = new Short(b);
            var inputCin = new Bit(cin);

            // Act
            (Short Sum, Bit Carry) actual = new ADDER<Short>(inputA, inputB, inputCin);

            // Assert
            Assert.That(actual.Sum, Is.EqualTo(expectedSum),
                $"Failed for A={a:X4}, B={b:X4}, Cin={cin}");
            Assert.That(actual.Carry, Is.EqualTo((Bit)(int)expectedCarry),
                $"Failed for A={a:X4}, B={b:X4}, Cin={cin}");
        }
    }

    // ==========================================
    // INT TESTS - Selected Cases with Dynamic Calculation
    // ==========================================

    [Test]
    public void ADDER_ImplicitConversion_WithIntInputs_SelectedCases_ReturnsCorrectOutput()
    {
        (uint, uint, Bit)[] testCases =
        [
            (0x00000000, 0x00000000, 0),
            (0xFFFFFFFF, 0xFFFFFFFF, 0),
            (0xFFFFFFFF, 0xFFFFFFFF, 1),
            (0xAAAAAAAA, 0x55555555, 0),
            (0xAAAAAAAA, 0xAAAAAAAA, 0),
            (0xAAAAAAAA, 0x00000000, 1),
            (0x12345678, 0x87654321, 0),
            (0x12345678, 0x87654321, 1),
            (0xFFFFFFFF, 0x00000000, 1),
            (0x80000000, 0x80000000, 0),
            (0x80000000, 0x80000000, 1),
        ];

        foreach (var (a, b, cin) in testCases)
        {
            // Calculate expected using long to avoid overflow
            long total = (long)a + b + cin;
            var expectedSum = new Int((int)(total & 0xFFFFFFFF));
            var expectedCarry = new Int((int)((total >> 32) & 0xFFFFFFFF));

            // Arrange
            var inputA = new Int(a);
            var inputB = new Int(b);

            // Act
            (Int Sum, Bit Carry) actual = new ADDER<Int>(inputA, inputB, cin);

            // Assert
            Assert.That(actual.Sum, Is.EqualTo(expectedSum),
                $"Failed for A={a:X8}, B={b:X8}, Cin={cin}");
            Assert.That(actual.Carry, Is.EqualTo((Bit)(int)expectedCarry),
                $"Failed for A={a:X8}, B={b:X8}, Cin={cin}");
        }
    }

    [Test]
    public void ADDER_ImplicitConversion_WithIntInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            int a = random.Next();
            int b = random.Next();
            int cin = random.Next(0, 2);

            // Calculate expected using long to avoid overflow
            long total = (long)a + b + cin;
            var expectedSum = new Int((int)(total & 0xFFFFFFFF));
            var expectedCarry = new Int((int)((total >> 32) & 0xFFFFFFFF));

            // Arrange
            var inputA = new Int(a);
            var inputB = new Int(b);
            var inputCin = new Bit(cin);

            // Act
            (Int Sum, Bit Carry) actual = new ADDER<Int>(inputA, inputB, inputCin);

            // Assert
            Assert.That(actual.Sum, Is.EqualTo(expectedSum),
                $"Failed for A={a:X8}, B={b:X8}, Cin={cin}");
            Assert.That(actual.Carry, Is.EqualTo((Bit)(int)expectedCarry),
                $"Failed for A={a:X8}, B={b:X8}, Cin={cin}");
        }
    }

    // ==========================================
    // LONG TESTS - Selected Cases with Dynamic Calculation
    // ==========================================

    [Test]
    public void ADDER_ImplicitConversion_WithLongInputs_SelectedCases_ReturnsCorrectOutput()
    {
        (ulong, ulong, Bit)[] testCases =
        [
            (0x0000000000000000UL, 0x0000000000000000UL, 0),
            (0xFFFFFFFFFFFFFFFFUL, 0xFFFFFFFFFFFFFFFFUL, 0),
            (0xFFFFFFFFFFFFFFFFUL, 0xFFFFFFFFFFFFFFFFUL, 1),
            (0xAAAAAAAAAAAAAAAAL, 0x5555555555555555L, 0),
            (0xAAAAAAAAAAAAAAAAL, 0xAAAAAAAAAAAAAAAAL, 0),
            (0xAAAAAAAAAAAAAAAAL, 0x0000000000000000L, 1),
            (0x123456789ABCDEF0L, 0xFEDCBA987654321FL, 0),
            (0x123456789ABCDEF0L, 0xFEDCBA987654321FL, 1),
            (0xFFFFFFFFFFFFFFFFL, 0x0000000000000000L, 1),
            (0x8000000000000000L, 0x8000000000000000L, 0),
            (0x8000000000000000L, 0x8000000000000000L, 1),
        ];

        foreach (var (a, b, cin) in testCases)
        {
            // Calculate expected with proper overflow detection
            ulong sumAB = a + b;
            bool overflowAB = a > ulong.MaxValue - b;

            // Now add cin
            ulong sum = sumAB + (ulong)(int)cin;
            bool overflow = overflowAB || ((ulong)(int)cin > ulong.MaxValue - sumAB);

            ulong carry = overflow ? 1UL : 0UL;

            var expectedSum = new Long((long)(sum & 0xFFFFFFFFFFFFFFFFUL));
            var expectedCarry = new Long((long)carry);

            // Arrange
            var inputA = new Long(a);
            var inputB = new Long(b);

            // Act
            (Long Sum, Bit Carry) actual = new ADDER<Long>(inputA, inputB, cin);

            // Assert
            Assert.That(actual.Sum, Is.EqualTo(expectedSum),
                $"Failed for A={a:X16}, B={b:X16}, Cin={cin}");
            Assert.That(actual.Carry, Is.EqualTo((Bit)expectedCarry),
                $"Failed for A={a:X16}, B={b:X16}, Cin={cin}");
        }
    }

    [Test]
    public void ADDER_ImplicitConversion_WithLongInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            ulong a = ((ulong)random.Next() << 32) | (uint)random.Next();
            ulong b = ((ulong)random.Next() << 32) | (uint)random.Next();
            Bit cin = (Bit)random.Next(0, 2);

            // Calculate expected
            ulong total = a + b + (ulong)(int)cin;
            ulong sum = total & 0xFFFFFFFFFFFFFFFFUL;

            // Carry is 1 if overflow occurred (total >= 2^64)
            ulong carry = (total < a || total < b) ? 1UL : 0UL;

            var expectedSum = new Long((long)sum);
            var expectedCarry = new Long((long)carry);

            // Arrange
            var inputA = new Long((long)a);
            var inputB = new Long((long)b);

            // Act
            (Long Sum, Bit Carry) actual = new ADDER<Long>(inputA, inputB, cin);

            // Assert
            Assert.That(actual.Sum, Is.EqualTo(expectedSum),
                $"Failed for A={a:X16}, B={b:X16}, Cin={cin}");
            Assert.That(actual.Carry, Is.EqualTo((Bit)expectedCarry),
                $"Failed for A={a:X16}, B={b:X16}, Cin={cin}");
        }
    }

    [Test]
    public void ADDER_ImplicitConversion_WithMixedTypes_CompilesAndWorks()
    {
        // Bit to Byte promotion
        var bitA = new Bit(true);
        var bitB = new Bit(false);
        var bitCin = new Bit(true);
        (Byte Sum, Bit Carry) actual1 = new ADDER<Byte>((int)bitA, (int)bitB, (int)bitCin);
        Assert.That(actual1.Sum, Is.EqualTo(new Byte(0x02))); // 1 + 0 + 1 = 2
        Assert.That(actual1.Carry, Is.EqualTo(new Bit(0x00))); // no carry

        // Byte to Short promotion
        var byteA = new Byte(0xFF);
        var byteB = new Byte(0x01);
        var byteCin = new Bit(0x00);
        (Short Sum, Bit Carry) actual2 = new ADDER<Short>(byteA, byteB, byteCin);
        Assert.That(actual2.Sum, Is.EqualTo(new Short(0x0100))); // 255 + 1 = 256
        Assert.That(actual2.Carry, Is.EqualTo(new Bit(0x0000)));

        // Short to Int promotion
        var shortA = new Short(0xFFFF);
        var shortB = new Short(0x0001);
        var shortCin = new Bit(0x0000);
        (Int Sum, Bit Carry) actual3 = new ADDER<Int>(shortA, shortB, shortCin);
        Assert.That(actual3.Sum, Is.EqualTo(new Int(0x00010000))); // 65535 + 1 = 65536
        Assert.That(actual3.Carry, Is.EqualTo(new Bit(0x00000000)));

        // Int to Long promotion
        var intA = new Int(0xFFFFFFFF);
        var intB = new Int(0x00000001);
        var intCin = new Bit(0x00000000);
        (Long Sum, Bit Carry) actual4 = new ADDER<Long>(intA, intB, intCin);
        Assert.That(actual4.Sum, Is.EqualTo(new Long(0x0000000100000000L))); // 4294967295 + 1 = 4294967296
        Assert.That(actual4.Carry, Is.EqualTo(new Bit(0x0)));
    }
}