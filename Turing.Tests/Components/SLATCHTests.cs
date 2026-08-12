using Turing.Core.Components.Memory;
using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

namespace Turing.Tests.Components.Memory;

[TestFixture]
internal class SLATCHTests
{
    // ==========================================
    // BIT TESTS - Exhaustive (2x2x2 = 8 cases per state)
    // ==========================================

    [Test]
    public void SLATCH_ImplicitConversion_WithBitInputs_AllStates_ReturnsCorrectOutput()
    {
        // Test all combinations of (initial, input, set)
        // Initial state is set using the constructor with Set=1
        for (int initial = 0; initial <= 1; initial++)
        {
            for (int input = 0; input <= 1; input++)
            {
                for (int set = 0; set <= 1; set++)
                {
                    // Arrange - set initial state using constructor with Set=1
                    var clock = new CLOCK();
                    clock.Set(new Bit(true));
                    var slatch = new SLATCH<Bit>(new Bit(initial), clock);

                    // Verify initial state was set correctly
                    var initialState = (Bit)slatch;

                    // Expected after EVal: if set=1, output=input; else output=initial
                    var expected = new Bit(set == 1 ? input : initial);

                    // Act
                    clock.Set(new Bit(set));
                    slatch.EVal(new Bit(input));
                    Bit actual = (Bit)slatch;

                    // Assert
                    Assert.That(actual, Is.EqualTo(expected),
                        $"Failed: Initial={initial}, Input={input}, Set={set}");
                }
            }
        }
    }

    [Test]
    public void SLATCH_Constructor_WithBitInputs_ReturnsCorrectOutput()
    {
        for (int input = 0; input <= 1; input++)
        {
            for (int set = 0; set <= 1; set++)
            {
                // Arrange
                var expected = new Bit(set == 1 ? input : 0);

                // Act
                var clock = new CLOCK();
                clock.Set(set);
                var slatch = new SLATCH<Bit>(new Bit(input), clock);
                Bit actual = (Bit)slatch;

                // Assert
                Assert.That(actual, Is.EqualTo(expected),
                    $"Failed: Input={input}, Set={set}");
            }
        }
    }

    [Test]
    public void SLATCH_Reset_WithBitInputs_ResetsToZero()
    {
        // Arrange
        var clock = new CLOCK();
        clock.Set(new Bit(true));
        var slatch = new SLATCH<Bit>(new Bit(true), clock); // Set to 1
        Assert.That((Bit)slatch, Is.EqualTo(new Bit(true)));

        // Act
        slatch.Reset();

        // Assert
        Assert.That((Bit)slatch, Is.EqualTo(new Bit(false)));
    }

    [Test]
    public void SLATCH_HoldsState_WhenSetIsFalse_WithBitInputs()
    {
        // Arrange
        var clock = new CLOCK();
        clock.Set(new Bit(true));
        var slatch = new SLATCH<Bit>(new Bit(true), clock); // Set to 1
        Assert.That((Bit)slatch, Is.EqualTo(new Bit(true)));

        // Act - try to change input while set is false
        clock.Set(new Bit(false));
        slatch.EVal(new Bit(false));

        // Assert - state should remain 1
        Assert.That((Bit)slatch, Is.EqualTo(new Bit(true)));
    }

    // ==========================================
    // BYTE TESTS - Exhaustive (256x2 = 512 cases per state)
    // ==========================================

    [Test]
    public void SLATCH_ImplicitConversion_WithByteInputs_AllStates_ReturnsCorrectOutput()
    {
        for (int initial = 0; initial < 256; initial++)
        {
            for (int input = 0; input < 256; input++)
            {
                for (int set = 0; set <= 1; set++)
                {
                    // Arrange - set initial state using constructor with Set=1
                    var clock = new CLOCK();
                    clock.Set(new Bit(true));
                    var slatch = new SLATCH<Byte>(new Byte(initial), clock);
                    var expected = new Byte(set == 1 ? input : initial);

                    // Act
                    clock.Set(new Bit(set));
                    slatch.EVal(new Byte(input));
                    Byte actual = (Byte)slatch;

                    // Assert
                    Assert.That(actual, Is.EqualTo(expected),
                        $"Failed: Initial={initial:X2}, Input={input:X2}, Set={set}");
                }
            }
        }
    }

    [Test]
    public void SLATCH_Constructor_WithByteInputs_ReturnsCorrectOutput()
    {
        for (int input = 0; input < 256; input++)
        {
            for (int set = 0; set <= 1; set++)
            {
                // Arrange
                var expected = new Byte(set == 1 ? input : 0);

                // Act
                var clock = new CLOCK();
                clock.Set(new Bit(set));
                var slatch = new SLATCH<Byte>(new Byte(input), clock);
                Byte actual = (Byte)slatch;

                // Assert
                Assert.That(actual, Is.EqualTo(expected),
                    $"Failed: Input={input:X2}, Set={set}");
            }
        }
    }

    [Test]
    public void SLATCH_WithByteInputs_SpecificCases_ReturnsCorrectOutput()
    {
        // Set = 1, load input
        var clock = new CLOCK();
        clock.Set(new Bit(true));
        var slatch1 = new SLATCH<Byte>(new Byte(0xAA), clock);
        Assert.That((Byte)slatch1, Is.EqualTo(new Byte(0xAA)));

        // Set = 0, hold state
        clock.Set(new Bit(false));
        slatch1.EVal(new Byte(0xCC));
        Assert.That((Byte)slatch1, Is.EqualTo(new Byte(0xAA)));

        // Set = 1, load new input
        clock.Set(new Bit(true));
        slatch1.EVal(new Byte(0xCC));
        Assert.That((Byte)slatch1, Is.EqualTo(new Byte(0xCC)));

        // Set = 0, hold state again
        clock.Set(new Bit(false));
        slatch1.EVal(new Byte(0xFF));
        Assert.That((Byte)slatch1, Is.EqualTo(new Byte(0xCC)));
    }

    [Test]
    public void SLATCH_Reset_WithByteInputs_ResetsToZero()
    {
        // Arrange
        var clock = new CLOCK();
        clock.Set(new Bit(true));
        var slatch = new SLATCH<Byte>(new Byte(0xAA), clock);
        Assert.That((Byte)slatch, Is.EqualTo(new Byte(0xAA)));

        // Act
        slatch.Reset();

        // Assert
        Assert.That((Byte)slatch, Is.EqualTo(new Byte(0x00)));
    }

    [Test]
    public void SLATCH_HoldsState_WhenSetIsFalse_WithByteInputs()
    {
        // Arrange
        var clock = new CLOCK();
        clock.Set(new Bit(true));
        var slatch = new SLATCH<Byte>(new Byte(0xAA), clock);
        Assert.That((Byte)slatch, Is.EqualTo(new Byte(0xAA)));

        // Act - try to change input while set is false
        clock.Set(new Bit(false));
        slatch.EVal(new Byte(0xCC));

        // Assert - state should remain 0xAA
        Assert.That((Byte)slatch, Is.EqualTo(new Byte(0xAA)));
    }

    // ==========================================
    // SHORT TESTS - Selected Cases
    // ==========================================

    [Test]
    public void SLATCH_WithShortInputs_SelectedCases_ReturnsCorrectOutput()
    {
        (int, int, int, int)[] testCases = new (int, int, int, int)[]
        {
            (0x0000, 0x0000, 0, 0x0000),
            (0x0000, 0x0000, 1, 0x0000),
            (0xAAAA, 0xAAAA, 0, 0xAAAA),
            (0xAAAA, 0xCCCC, 0, 0xAAAA),
            (0xAAAA, 0xCCCC, 1, 0xCCCC),
            (0xFFFF, 0x0000, 0, 0xFFFF),
            (0xFFFF, 0x0000, 1, 0x0000),
            (0x1234, 0x5678, 0, 0x1234),
            (0x1234, 0x5678, 1, 0x5678),
        };

        foreach (var (initial, input, set, expected) in testCases)
        {
            // Arrange
            var clock = new CLOCK();
            clock.Set(new Bit(true));
            var slatch = new SLATCH<Short>(new Short(initial), clock);
            var expectedOutput = new Short(expected);

            // Act
            clock.Set(new Bit(set));
            slatch.EVal(new Short(input));
            Short actual = (Short)slatch;

            // Assert
            Assert.That(actual, Is.EqualTo(expectedOutput),
                $"Failed: Initial={initial:X4}, Input={input:X4}, Set={set}");
        }
    }

    [Test]
    public void SLATCH_WithShortInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            int initial = random.Next(0x0000, 0xFFFF + 1);
            int input = random.Next(0x0000, 0xFFFF + 1);
            int set = random.Next(0, 2);

            // Arrange
            var clock = new CLOCK();
            clock.Set(new Bit(true));
            var slatch = new SLATCH<Short>(new Short(initial), clock);
            var expected = new Short(set == 1 ? input : initial);

            // Act
            clock.Set(new Bit(set));
            slatch.EVal(new Short(input));
            Short actual = (Short)slatch;

            // Assert
            Assert.That(actual, Is.EqualTo(expected),
                $"Failed: Initial={initial:X4}, Input={input:X4}, Set={set}");
        }
    }

    // ==========================================
    // INT TESTS - Selected Cases
    // ==========================================

    [Test]
    public void SLATCH_WithIntInputs_SelectedCases_ReturnsCorrectOutput()
    {
        (uint, uint, int, uint)[] testCases = new (uint, uint, int, uint)[]
        {
            (0x00000000, 0x00000000, 0, 0x00000000),
            (0x00000000, 0x00000000, 1, 0x00000000),
            (0xAAAAAAAA, 0xAAAAAAAA, 0, 0xAAAAAAAA),
            (0xAAAAAAAA, 0xCCCCCCCC, 0, 0xAAAAAAAA),
            (0xAAAAAAAA, 0xCCCCCCCC, 1, 0xCCCCCCCC),
            (0xFFFFFFFF, 0x00000000, 0, 0xFFFFFFFF),
            (0xFFFFFFFF, 0x00000000, 1, 0x00000000),
            (0x12345678, 0x87654321, 0, 0x12345678),
            (0x12345678, 0x87654321, 1, 0x87654321),
        };

        foreach (var (initial, input, set, expected) in testCases)
        {
            // Arrange
            var clock = new CLOCK();
            clock.Set(new Bit(true));
            var slatch = new SLATCH<Int>(new Int(initial), clock);
            var expectedOutput = new Int(expected);

            // Act
            clock.Set(new Bit(set));
            slatch.EVal(new Int(input));
            Int actual = (Int)slatch;

            // Assert
            Assert.That(actual, Is.EqualTo(expectedOutput),
                $"Failed: Initial={initial:X8}, Input={input:X8}, Set={set}");
        }
    }

    [Test]
    public void SLATCH_WithIntInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            int initial = random.Next();
            int input = random.Next();
            int set = random.Next(0, 2);

            // Arrange
            var clock = new CLOCK();
            clock.Set(new Bit(true));
            var slatch = new SLATCH<Int>(new Int(initial), clock);
            var expected = new Int(set == 1 ? input : initial);

            // Act           
            clock.Set(new Bit(set));
            slatch.EVal(new Int(input));
            Int actual = (Int)slatch;

            // Assert
            Assert.That(actual, Is.EqualTo(expected),
                $"Failed: Initial={initial:X8}, Input={input:X8}, Set={set}");
        }
    }

    // ==========================================
    // LONG TESTS - Selected Cases
    // ==========================================

    [Test]
    public void SLATCH_WithLongInputs_SelectedCases_ReturnsCorrectOutput()
    {
        (ulong, ulong, int, ulong)[] testCases = new (ulong, ulong, int, ulong)[]
        {
            (0x0000000000000000UL, 0x0000000000000000UL, 0, 0x0000000000000000UL),
            (0x0000000000000000UL, 0x0000000000000000UL, 1, 0x0000000000000000UL),
            (0xAAAAAAAAAAAAAAAAL, 0xAAAAAAAAAAAAAAAAL, 0, 0xAAAAAAAAAAAAAAAAL),
            (0xAAAAAAAAAAAAAAAAL, 0xCCCCCCCCCCCCCCCCL, 0, 0xAAAAAAAAAAAAAAAAL),
            (0xAAAAAAAAAAAAAAAAL, 0xCCCCCCCCCCCCCCCCL, 1, 0xCCCCCCCCCCCCCCCCL),
            (0xFFFFFFFFFFFFFFFFL, 0x0000000000000000L, 0, 0xFFFFFFFFFFFFFFFFL),
            (0xFFFFFFFFFFFFFFFFL, 0x0000000000000000L, 1, 0x0000000000000000L),
            (0x123456789ABCDEF0L, 0xFEDCBA987654321FL, 0, 0x123456789ABCDEF0L),
            (0x123456789ABCDEF0L, 0xFEDCBA987654321FL, 1, 0xFEDCBA987654321FL),
        };

        foreach (var (initial, input, set, expected) in testCases)
        {
            // Arrange
            var clock = new CLOCK();
            clock.Set(new Bit(true));
            var slatch = new SLATCH<Long>(new Long(initial), clock);
            var expectedOutput = new Long(expected);

            // Act
            clock.Set(new Bit(set));
            slatch.EVal(new Long(input));
            Long actual = (Long)slatch;

            // Assert
            Assert.That(actual, Is.EqualTo(expectedOutput),
                $"Failed: Initial={initial:X16}, Input={input:X16}, Set={set}");
        }
    }

    [Test]
    public void SLATCH_WithLongInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            long initial = ((long)random.Next() << 32) | (uint)random.Next();
            long input = ((long)random.Next() << 32) | (uint)random.Next();
            int set = random.Next(0, 2);

            // Arrange
            var clock = new CLOCK();
            clock.Set(new Bit(true));
            var slatch = new SLATCH<Long>(new Long(initial), clock);
            var expected = new Long(set == 1 ? input : initial);

            // Act
            clock.Set(new Bit(set));
            slatch.EVal(new Long(input));
            Long actual = (Long)slatch;

            // Assert
            Assert.That(actual, Is.EqualTo(expected),
                $"Failed: Initial={initial:X16}, Input={input:X16}, Set={set}");
        }
    }

    // ==========================================
    // TYPE PROMOTION TESTS
    // ==========================================

    [Test]
    public void SLATCH_WithMixedTypes_CompilesAndWorks()
    {
        // Bit to Byte promotion
        var clock = new CLOCK();

        var slatch1 = new SLATCH<Byte>(clock);
        clock.Set(new Bit(true));
        slatch1.EVal(new Bit(true));
        Byte actual1 = (Byte)slatch1;
        Assert.That(actual1, Is.EqualTo(new Byte(0x01)));

        clock.Set(new Bit(true));
        slatch1.EVal(new Bit(false));
        Byte actual2 = (Byte)slatch1;
        Assert.That(actual2, Is.EqualTo(new Byte(0x00)));

        // Byte to Short promotion
        clock.Set(new Bit(true));
        var slatch2 = new SLATCH<Short>(clock);
        slatch2.EVal(new Byte(0xAA));
        Short actual3 = (Short)slatch2;
        Assert.That(actual3, Is.EqualTo(new Short(0x00AA)));

        // Short to Int promotion
        clock.Set(new Bit(true));
        var slatch3 = new SLATCH<Int>(clock);
        slatch3.EVal(new Short(0xAAAA));
        Int actual4 = (Int)slatch3;
        Assert.That(actual4, Is.EqualTo(new Int(0x0000AAAA)));

        // Int to Long promotion
        clock.Set(new Bit(true));
        var slatch4 = new SLATCH<Long>(clock);
        slatch4.EVal(new Int(0xAAAAAAAA));
        Long actual5 = (Long)slatch4;
        Assert.That(actual5, Is.EqualTo(new Long(0x00000000AAAAAAAA)));
    }
}