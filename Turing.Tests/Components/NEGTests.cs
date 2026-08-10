using Turing.Core.Components.Arithmetic;

namespace Turing.Tests.Components;

[TestFixture]
internal class NEGTests
{
    // ==========================================
    // BYTE TESTS
    // ==========================================

    [TestCase(0, 0)]
    [TestCase(1, 255)]
    [TestCase(2, 254)]
    [TestCase(127, 129)]
    [TestCase(128, 128)]   // -128 == 128 (two's complement, no signed equivalent)
    [TestCase(255, 1)]     // -1 == 255
    public void NEG_Byte_ReturnsCorrectNegation(int value, int expected)
    {
        // Arrange
        Byte input = new Byte(value);
        Byte expectedResult = new Byte(expected);

        // Act
        NEG<Byte> neg = new NEG<Byte>(input);
        Byte result = (Byte)neg;

        // Assert
        Assert.That(result, Is.EqualTo(expectedResult),
            $"Negation of {value} should be {expected}");
    }

    // ==========================================
    // SHORT TESTS
    // ==========================================

    [TestCase(0, 0)]
    [TestCase(1, 65535)]
    [TestCase(2, 65534)]
    [TestCase(32767, 32769)]
    [TestCase(32768, 32768)]   // -32768
    [TestCase(65535, 1)]       // -1
    public void NEG_Short_ReturnsCorrectNegation(int value, int expected)
    {
        // Arrange
        Short input = new Short(value);
        Short expectedResult = new Short(expected);

        // Act
        NEG<Short> neg = new NEG<Short>(input);
        Short result = (Short)neg;

        // Assert
        Assert.That(result, Is.EqualTo(expectedResult),
            $"Negation of {value} should be {expected}");
    }

    // ==========================================
    // INT TESTS
    // ==========================================

    [Test]
    public void NEG_Int_ReturnsCorrectNegation()
    {
        int[] values = new int[] { 0, 1, 2, int.MaxValue, int.MinValue, -1 };
        foreach (int value in values)
        {
            // Compute expected using checked arithmetic for edge cases
            int expected = unchecked(-value); // use unchecked to allow overflow (int.MinValue -> int.MinValue)
            Int input = new Int(value);
            NEG<Int> neg = new NEG<Int>(input);
            Int result = (Int)neg;
            Assert.That((int)result, Is.EqualTo(expected),
                $"Negation of {value:X8} should be {expected:X8}");
        }
    }

    [Test]
    public void NEG_Int_RandomValues_ReturnsCorrectNegation()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            int value = random.Next();
            int expected = unchecked(-value);
            Int input = new Int(value);
            NEG<Int> neg = new NEG<Int>(input);
            Int result = (Int)neg;
            Assert.That((int)result, Is.EqualTo(expected),
                $"Negation of {value:X8} should be {expected:X8}");
        }
    }

    // ==========================================
    // LONG TESTS
    // ==========================================

    [Test]
    public void NEG_Long_ReturnsCorrectNegation()
    {
        long[] values = new long[] { 0L, 1L, 2L, long.MaxValue, long.MinValue, -1L };
        foreach (long value in values)
        {
            long expected = unchecked(-value);
            Long input = new Long(value);
            NEG<Long> neg = new NEG<Long>(input);
            Long result = (Long)neg;
            Assert.That((long)result, Is.EqualTo(expected),
                $"Negation of {value:X16} should be {expected:X16}");
        }
    }

    [Test]
    public void NEG_Long_RandomValues_ReturnsCorrectNegation()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            long value = ((long)random.Next() << 32) | (uint)random.Next();
            long expected = unchecked(-value);
            Long input = new Long(value);
            NEG<Long> neg = new NEG<Long>(input);
            Long result = (Long)neg;
            Assert.That((long)result, Is.EqualTo(expected),
                $"Negation of {value:X16} should be {expected:X16}");
        }
    }

    // ==========================================
    // EDGE CASES
    // ==========================================

    [Test]
    public void NEG_Zero_ReturnsZero()
    {
        NEG<Byte> neg = new NEG<Byte>(new Byte(0));
        Assert.That((Byte)neg, Is.EqualTo(new Byte(0)));
    }

    [Test]
    public void NEG_MinValue_ReturnsMinValue()
    {
        // 8‑bit: -128 → 128 (stored as unsigned 128)
        NEG<Byte> negB = new NEG<Byte>(new Byte(128));
        Assert.That((Byte)negB, Is.EqualTo(new Byte(128)));

        // 16‑bit: -32768 → 32768
        NEG<Short> negS = new NEG<Short>(new Short(32768));
        Assert.That((Short)negS, Is.EqualTo(new Short(32768)));

        // 32‑bit: int.MinValue → int.MinValue
        NEG<Int> negI = new NEG<Int>(new Int(int.MinValue));
        Assert.That((Int)negI, Is.EqualTo(new Int(int.MinValue)));

        // 64‑bit: long.MinValue → long.MinValue
        NEG<Long> negL = new NEG<Long>(new Long(long.MinValue));
        Assert.That((Long)negL, Is.EqualTo(new Long(long.MinValue)));
    }
}