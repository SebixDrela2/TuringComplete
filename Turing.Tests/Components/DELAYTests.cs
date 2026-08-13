using Turing.Core.Components.Memory;
using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;

namespace Turing.Tests.Components.Memory;

[TestFixture]
internal class DELAYTests
{
    // ==========================================
    // BIT TESTS - Sequential Test Cases (Corrected for Positive-Edge Flip-Flop)
    // ==========================================

    [TestCase(0, 0, 0, 0, 0)]
    [TestCase(0, 0, 0, 0, 1)]
    [TestCase(0, 0, 0, 1, 0)]
    [TestCase(0, 0, 0, 1, 1)]
    [TestCase(0, 0, 1, 0, 0)]
    [TestCase(0, 0, 1, 0, 1)]
    [TestCase(0, 0, 1, 1, 0)]
    [TestCase(0, 0, 1, 1, 1)]
    [TestCase(0, 1, 0, 0, 0)]
    [TestCase(0, 1, 0, 0, 1)]
    [TestCase(0, 1, 0, 1, 0)]
    [TestCase(0, 1, 0, 1, 1)]
    [TestCase(0, 1, 1, 0, 0)]
    [TestCase(0, 1, 1, 0, 1)]
    [TestCase(0, 1, 1, 1, 0)]
    [TestCase(0, 1, 1, 1, 1)]
    [TestCase(1, 0, 0, 0, 0)]
    [TestCase(1, 0, 0, 0, 1)]
    [TestCase(1, 0, 0, 1, 0)]
    [TestCase(1, 0, 0, 1, 1)]
    [TestCase(1, 0, 1, 0, 0)]
    [TestCase(1, 0, 1, 0, 1)]
    [TestCase(1, 0, 1, 1, 0)]
    [TestCase(1, 0, 1, 1, 1)]
    [TestCase(1, 1, 0, 0, 0)]
    [TestCase(1, 1, 0, 0, 1)]
    [TestCase(1, 1, 0, 1, 0)]
    [TestCase(1, 1, 0, 1, 1)]
    [TestCase(1, 1, 1, 0, 0)]
    [TestCase(1, 1, 1, 0, 1)]
    [TestCase(1, 1, 1, 1, 0)]
    [TestCase(1, 1, 1, 1, 1)]
    public void DELAY_Sequential_WithBitInputs_ReturnsCorrectOutput(
        int tick1, int tick2, int tick3, int tick4, int tick5)
    {
        var clock = new CLOCK();
        var delay = new DELAY<Bit>(clock);
        Bit captured = new Bit(0); // Last value captured on rising edge

        // Step 1: Clock=0 (low), input=tick1
        
        delay.EVal(new Bit(tick1));
        clock.Tick();
        Bit result1 = (Bit)delay;
        // On clock low, output holds the last captured value (initial 0)
        Assert.That(result1, Is.EqualTo(captured),
            $"Step1 failed: expected {captured.Value}, got {result1.Value}");
        // No rising edge, captured unchanged

        // Step 2: Clock=1 (rising edge), input=tick2
        
        delay.EVal(new Bit(tick2));
        clock.Tick();
        Bit result2 = (Bit)delay;
        // On rising edge, output becomes input
        captured = new Bit(tick2);
        Assert.That(result2, Is.EqualTo(captured),
            $"Step2 failed: expected {captured.Value}, got {result2.Value}");

        // Step 3: Clock=0 (low), input=tick3
        
        delay.EVal(new Bit(tick3));
        clock.Tick();
        Bit result3 = (Bit)delay;
        // On clock low, output holds the last captured value (tick2)
        Assert.That(result3, Is.EqualTo(captured),
            $"Step3 failed: expected {captured.Value}, got {result3.Value}");
        // No rising edge

        // Step 4: Clock=1 (rising edge), input=tick4
        
        delay.EVal(new Bit(tick4));
        clock.Tick();
        Bit result4 = (Bit)delay;
        // On rising edge, output becomes input
        captured = new Bit(tick4);
        Assert.That(result4, Is.EqualTo(captured),
            $"Step4 failed: expected {captured.Value}, got {result4.Value}");

        // Step 5: Clock=0 (low), input=tick5
        
        delay.EVal(new Bit(tick5));
        clock.Tick();
        Bit result5 = (Bit)delay;
        // On clock low, output holds the last captured value (tick4)
        Assert.That(result5, Is.EqualTo(captured),
            $"Step5 failed: expected {captured.Value}, got {result5.Value}");
    }

    // ==========================================
    // BIT TESTS - Exhaustive All States
    // ==========================================

    [Test]
    public void DELAY_Constructor_WithBitInputs_ReturnsCorrectOutput()
    {
        for (int input = 0; input <= 1; input++)
        {
            var clock = new CLOCK();
            var delay = new DELAY<Bit>(clock);

            for (int tick = 0; tick <= 1; tick++)
            {
                var expected = new Bit(tick == 1 ? input : 0);
                delay.EVal(new Bit(input));
                Bit actual = (Bit)delay;

                Assert.That(actual, Is.EqualTo(expected),
                    $"Failed: Input={input}, Tick={tick}");
                clock.Tick();
            }
        }
    }

    [Test]
    public void DELAY_Reset_WithBitInputs_ResetsToZero()
    {
        var clock = new CLOCK();
        var delay = new DELAY<Bit>(clock);

        
        delay.EVal(new Bit(1));
        clock.Tick();
        
        delay.EVal(new Bit(1));
        clock.Tick();
        Assert.That((Bit)delay, Is.EqualTo(new Bit(1)));

        delay.Reset();
        Assert.That((Bit)delay, Is.EqualTo(new Bit(0)));
    }

    // ==========================================
    // BYTE TESTS - Exhaustive
    // ==========================================

    [Test]
    public void DELAY_ImplicitConversion_WithByteInputs_AllStates_ReturnsCorrectOutput()
    {
        for (int initial = 0; initial < 256; initial++)
        {
            for (int input = 0; input < 256; input++)
            {
                var clock = new CLOCK();
                var delay = new DELAY<Byte>(clock);

                for (int tick = 0; tick <= 1; tick++)
                {               
                    delay.EVal(new Byte(initial));
                    clock.Tick();
                    
                    delay.EVal(new Byte(initial));
                    clock.Tick();
                    
                    delay.EVal(new Byte(input));
                    clock.Tick();
                    var expected = new Byte(tick == 1 ? input : initial);
                    
                    delay.EVal(new Byte(input));
                    clock.Tick();
                    Byte actual = (Byte)delay;

                    Assert.That(actual, Is.EqualTo(expected),
                        $"Failed: Initial={initial:X2}, Input={input:X2}, Tick={tick}");
                    clock.Tick();
                }
            }
        }
    }

    [Test]
    public void DELAY_Constructor_WithByteInputs_ReturnsCorrectOutput()
    {
        for (int input = 0; input < 256; input++)
        {
            var clock = new CLOCK();
            var delay = new DELAY<Byte>(clock);

            for (int tick = 0; tick <= 1; tick++)
            {
                var expected = new Byte(tick == 1 ? input : 0);
                delay.EVal(new Byte(input));
                Byte actual = (Byte)delay;

                Assert.That(actual, Is.EqualTo(expected),
                    $"Failed: Input={input:X2}, Tick={tick}");
                clock.Tick();
            }
        }
    }

    [Test]
    public void DELAY_WithByteInputs_SpecificCases_ReturnsCorrectOutput()
    {
        var clock = new CLOCK();
        var delay = new DELAY<Byte>(clock);

        delay.EVal(new Byte(0xAA));
        clock.Tick();
        Assert.That((Byte)delay, Is.EqualTo(new Byte(0x00)));

        delay.EVal(new Byte(0xAA));
        clock.Tick();
        Assert.That((Byte)delay, Is.EqualTo(new Byte(0xAA)));

        delay.EVal(new Byte(0xCC));
        clock.Tick();
        Assert.That((Byte)delay, Is.EqualTo(new Byte(0xAA)));

        delay.EVal(new Byte(0xCC));
        clock.Tick();
        Assert.That((Byte)delay, Is.EqualTo(new Byte(0xCC)));

        delay.EVal(new Byte(0xFF));
        clock.Tick();
        Assert.That((Byte)delay, Is.EqualTo(new Byte(0xCC)));
    }

    [Test]
    public void DELAY_Reset_WithByteInputs_ResetsToZero()
    {
        var clock = new CLOCK();
        var delay = new DELAY<Byte>(clock);

        
        delay.EVal(new Byte(0xAA));
        clock.Tick();
        
        delay.EVal(new Byte(0xAA));
        clock.Tick();
        Assert.That((Byte)delay, Is.EqualTo(new Byte(0xAA)));

        delay.Reset();
        Assert.That((Byte)delay, Is.EqualTo(new Byte(0x00)));
    }

    // ==========================================
    // SHORT TESTS - Random Cases
    // ==========================================

    [Test]
    public void DELAY_WithShortInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            int initial = random.Next(0x0000, 0xFFFF + 1);
            int input = random.Next(0x0000, 0xFFFF + 1);
            int tick = random.Next(0, 2);

            var clock = new CLOCK();
            var delay = new DELAY<Short>(clock);
            
            delay.EVal(new Short(initial));
            clock.Tick();
            
            delay.EVal(new Short(initial));
            clock.Tick();
            
            delay.EVal(new Short(input));
            clock.Tick();
            var expected = new Short(tick == 1 ? input : initial);

            
            delay.EVal(new Short(input));
            clock.Tick();
            Short actual = (Short)delay;

            Assert.That(actual, Is.EqualTo(expected),
                $"Failed: Initial={initial:X4}, Input={input:X4}, Tick={tick}");
        }
    }

    // ==========================================
    // INT TESTS - Random Cases
    // ==========================================

    [Test]
    public void DELAY_WithIntInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            int initial = random.Next();
            int input = random.Next();
            int tick = random.Next(0, 2);

            var clock = new CLOCK();
            
            var delay = new DELAY<Int>(clock);
            clock.Tick();

            
            delay.EVal(new Int(initial));
            clock.Tick();
            
            delay.EVal(new Int(input));
            clock.Tick();
            var expected = new Int(tick == 1 ? input : initial);

            
            delay.EVal(new Int(input));
            clock.Tick();
            Int actual = (Int)delay;

            Assert.That(actual, Is.EqualTo(expected),
                $"Failed: Initial={initial:X8}, Input={input:X8}, Tick={tick}");
        }
    }

    // ==========================================
    // LONG TESTS - Random Cases
    // ==========================================

    [Test]
    public void DELAY_WithLongInputs_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            long initial = ((long)random.Next() << 32) | (uint)random.Next();
            long input = ((long)random.Next() << 32) | (uint)random.Next();
            int tick = random.Next(0, 2);

            var clock = new CLOCK();
            
            var delay = new DELAY<Long>(clock);
            clock.Tick();
            
            delay.EVal(new Long(initial));
            clock.Tick();
            
            delay.EVal(new Long(input));
            clock.Tick();
            var expected = new Long(tick == 1 ? input : initial);

            
            delay.EVal(new Long(input));
            clock.Tick();
            Long actual = (Long)delay;

            Assert.That(actual, Is.EqualTo(expected),
                $"Failed: Initial={initial:X16}, Input={input:X16}, Tick={tick}");
        }
    }

    // ==========================================
    // TYPE PROMOTION TESTS
    // ==========================================

    [Test]
    public void DELAY_WithMixedTypes_CompilesAndWorks()
    {
        var clock = new CLOCK();
        var delay1 = new DELAY<Byte>(clock);

        
        delay1.EVal(new Bit(true));
        clock.Tick();
        Byte actual1 = (Byte)delay1;
        Assert.That(actual1, Is.EqualTo(new Byte(0x01)));

        
        delay1.EVal(new Bit(false));
        clock.Tick();
        Byte actual2 = (Byte)delay1;
        Assert.That(actual2, Is.EqualTo(new Byte(0x00)));

        
        var delay2 = new DELAY<Short>(clock);
        clock.Tick();
        
        delay2.EVal(new Byte(0xAA));
        clock.Tick();
        Short actual3 = (Short)delay2;
        Assert.That(actual3, Is.EqualTo(new Short(0x00AA)));

        var delay3 = new DELAY<Int>(clock);
        
        delay3.EVal(new Short(0xAAAA));
        clock.Tick();
        Int actual4 = (Int)delay3;
        Assert.That(actual4, Is.EqualTo(new Int(0x0000AAAA)));

        var delay4 = new DELAY<Long>(clock);
        
        delay4.EVal(new Int(0xAAAAAAAA));
        clock.Tick();
        Long actual5 = (Long)delay4;
        Assert.That(actual5, Is.EqualTo(new Long(0x00000000AAAAAAAA)));
    }
}