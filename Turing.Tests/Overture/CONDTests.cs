using Turing.Core.Overture;
using Turing.Core.Electricity;

namespace Turing.Tests.Overture;

[TestFixture]
internal class CONDTests
{
    [TestCase(0, 0, false)]
    [TestCase(1, 0, true)]
    [TestCase(2, 0, true)]
    [TestCase(3, 0, false)]
    [TestCase(4, 0, false)]
    [TestCase(5, 0, true)]
    [TestCase(6, 0, true)]
    [TestCase(7, 0, false)]
    public void COND_WithZero_ReturnsCorrectResult(int condCode, int value, bool expectedResult)
    {
        Byte condition = new Byte(condCode); // code packed in bits 0..2
        Byte inputValue = new Byte(value);
        var cond = new COND(inputValue, condition);
        Bit result = (Bit)cond;
        Assert.That(result, Is.EqualTo(new Bit(expectedResult)),
            $"CondCode={condCode}, Value={value}");
    }

    [TestCase(0, 42, false)]
    [TestCase(1, 42, true)]
    [TestCase(2, 42, false)]
    [TestCase(3, 42, true)]
    [TestCase(4, 42, false)]
    [TestCase(5, 42, true)]
    [TestCase(6, 42, false)]
    [TestCase(7, 42, true)]
    public void COND_WithPositiveValue_ReturnsCorrectResult(int condCode, int value, bool expectedResult)
    {
        Byte condition = new Byte(condCode);
        Byte inputValue = new Byte(value);
        var cond = new COND(inputValue, condition);
        Bit result = (Bit)cond;
        Assert.That(result, Is.EqualTo(new Bit(expectedResult)),
            $"CondCode={condCode}, Value={value}");
    }

    [TestCase(0, -42, false)]
    [TestCase(1, -42, true)]
    [TestCase(2, -42, false)]
    [TestCase(3, -42, true)]
    [TestCase(4, -42, true)]
    [TestCase(5, -42, false)]
    [TestCase(6, -42, true)]
    [TestCase(7, -42, false)]
    public void COND_WithNegativeValue_ReturnsCorrectResult(int condCode, int value, bool expectedResult)
    {
        Byte condition = new Byte(condCode);
        Byte inputValue = new Byte((byte)value);
        var cond = new COND(inputValue, condition);
        Bit result = (Bit)cond;
        Assert.That(result, Is.EqualTo(new Bit(expectedResult)),
            $"CondCode={condCode}, Value={value}");
    }

    [Test]
    public void COND_AllValues_ReturnsCorrectResults()
    {
        for (int value = -128; value <= 127; value++)
        {
            for (int condCode = 0; condCode <= 7; condCode++)
            {
                Byte condition = new Byte(condCode);
                Byte inputValue = new Byte((byte)value);
                bool expected = GetExpectedResult(condCode, value);
                var cond = new COND(inputValue, condition);
                Bit result = (Bit)cond;
                Assert.That(result, Is.EqualTo(new Bit(expected)),
                    $"CondCode={condCode}, Value={value}");
            }
        }
    }

    [Test]
    public void COND_ConditionByte_UsesOnlyBits0_1_2()
    {
        for (int bitMask = 0; bitMask < 0b11111000; bitMask += 8) // vary higher bits
        {
            // Condition code 1 (Always) at bits 0,1,2 (c0=1, c1=0, c2=0)
            int condByte = 0b001 | bitMask; // code 1
            Byte condition = new Byte(condByte);

            var cond1 = new COND(new Byte(0), condition);
            var cond2 = new COND(new Byte(42), condition);
            var cond3 = new COND(new Byte(-1), condition);

            Assert.That((Bit)cond1, Is.EqualTo(new Bit(true)), "Always should return true for value 0");
            Assert.That((Bit)cond2, Is.EqualTo(new Bit(true)), "Always should return true for value 42");
            Assert.That((Bit)cond3, Is.EqualTo(new Bit(true)), "Always should return true for value -1");
        }
    }

    [Test]
    public void COND_RandomValues_ReturnsCorrectResults()
    {
        var random = new Random(31337);
        for (int cycle = 0; cycle < 2048; cycle++)
        {
            int x = cycle;
            x ^= x << 1;
            x ^= x >> 5;
            x ^= x << 8;

            int condCode = x & 0b111;
            int value = (x >> 3) & 0xFF;
            if ((value & 0x80) != 0)
                value = -((value ^ 0xFF) + 1);

            Byte condition = new Byte(condCode);
            Byte inputValue = new Byte((byte)value);
            bool expected = GetExpectedResult(condCode, value);
            var cond = new COND(inputValue, condition);
            Bit result = (Bit)cond;
            Assert.That(result, Is.EqualTo(new Bit(expected)),
                $"Cycle={cycle}, CondCode={condCode}, Value={value}");
        }
    }

    private static bool GetExpectedResult(int condCode, int value)
    {
        return condCode switch
        {
            0 => false,           // Never
            1 => true,            // Always
            2 => value == 0,      // == 0
            3 => value != 0,      // != 0
            4 => value < 0,       // < 0
            5 => value >= 0,      // >= 0
            6 => value <= 0,      // <= 0
            7 => value > 0,       // > 0
            _ => false
        };
    }
}