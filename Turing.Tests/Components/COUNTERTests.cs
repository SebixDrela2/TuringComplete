using Turing.Core.Components.Memory;
using Turing.Core.Electricity;
using Turing.Core.Gates.Primitives;
using Turing.Core.Overture;

namespace Turing.Tests.Components.Memory;

[TestFixture]
internal class COUNTERTests
{
    // Helper: perform a full clock cycle that updates the counter state.
    // Assumes clock is LOW at the start (we set it explicitly inside).
    private void CycleCounter<T>(COUNTER<T> counter, Bit load, T loadValue) where T : struct, IValue<T>
    {
        counter._clock.Set(new Bit(false));
        counter.EVal(load, loadValue);
        counter._clock.Set(Bit.One);
        counter.EVal(load, loadValue);
        counter._clock.Set(new Bit(false));
        counter.EVal(load, loadValue);
    }

    // ==========================================
    // BIT COUNTER TESTS
    // ==========================================

    [Test]
    public void COUNTER_Bit_CountsCorrectly()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Bit>(clock);
        Assert.That((Bit)counter, Is.EqualTo(new Bit(0)));

        CycleCounter(counter, new Bit(false), new Bit(0));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(1)));

        CycleCounter(counter, new Bit(false), new Bit(0));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(0)));

        CycleCounter(counter, new Bit(false), new Bit(0));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(1)));
    }

    [Test]
    public void COUNTER_Bit_Load_LoadsValue()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Bit>(clock);

        CycleCounter(counter, Bit.One, new Bit(1));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(1)));

        CycleCounter(counter, new Bit(false), new Bit(0));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(0)));

        CycleCounter(counter, Bit.One, new Bit(1));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(1)));
    }

    [Test]
    public void COUNTER_Bit_Load_OnlyOnTick()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Bit>(clock);

        clock.Set(new Bit(false));
        counter.EVal(Bit.One, new Bit(1));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(0)));

        // Create a full cycle to update
        clock.Set(Bit.One);
        counter.EVal(Bit.One, new Bit(1));
        clock.Set(new Bit(false));
        counter.EVal(Bit.One, new Bit(1));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(1)));

        clock.Set(new Bit(false));
        counter.EVal(Bit.One, new Bit(0));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(1)));

        clock.Set(Bit.One);
        counter.EVal(Bit.One, new Bit(0));
        clock.Set(new Bit(false));
        counter.EVal(Bit.One, new Bit(0));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(0)));
    }

    [Test]
    public void COUNTER_Bit_Reset_ResetsToZero()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Bit>(clock);
        CycleCounter(counter, Bit.One, new Bit(1));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(1)));

        counter.Reset();
        // Force a cycle to latch the reset value (0)
        CycleCounter(counter, Bit.One, new Bit(0));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(0)));
    }

    [Test]
    public void COUNTER_Bit_AllStates_ReturnsCorrectOutput()
    {
        for (int init = 0; init <= 1; init++)
        {
            for (int load = 0; load <= 1; load++)
            {
                for (int loadValue = 0; loadValue <= 1; loadValue++)
                {
                    for (int tick = 0; tick <= 1; tick++)
                    {
                        var clock = new CLOCK();
                        var counter = new COUNTER<Bit>(clock); // start at 0

                        // Load the initial value using a full cycle
                        CycleCounter(counter, Bit.One, new Bit(init));
                        // Now counter.State == init

                        Bit expected;
                        if (tick == 0)
                        {
                            expected = new Bit(init);
                            // No clock edge – just call EVal (does nothing)
                            counter.EVal(new Bit(load), new Bit(loadValue));
                        }
                        else
                        {
                            // Perform a cycle with the given load and loadValue
                            if (load == 1)
                                expected = new Bit(loadValue);
                            else
                                expected = new Bit(init == 0 ? 1 : 0);

                            CycleCounter(counter, new Bit(load), new Bit(loadValue));
                        }

                        Bit actual = (Bit)counter;
                        Assert.That(actual, Is.EqualTo(expected),
                            $"Init={init}, Load={load}, LoadValue={loadValue}, Tick={tick}");
                    }
                }
            }
        }
    }

    // ==========================================
    // BYTE COUNTER TESTS
    // ==========================================

    [Test]
    public void COUNTER_Byte_CountsCorrectly()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Byte>(clock);
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0)));

        for (int i = 1; i <= 10; i++)
        {
            CycleCounter(counter, new Bit(false), new Byte(0));
            Assert.That((Byte)counter, Is.EqualTo(new Byte(i)));
        }
    }

    [Test]
    public void COUNTER_Byte_Load_LoadsValue()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Byte>(clock);

        for (int i = 1; i <= 5; i++)
            CycleCounter(counter, new Bit(false), new Byte(0));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(5)));

        CycleCounter(counter, Bit.One, new Byte(0xAA));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xAA)));

        CycleCounter(counter, new Bit(false), new Byte(0));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xAB)));

        CycleCounter(counter, Bit.One, new Byte(0x00));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0x00)));

        CycleCounter(counter, new Bit(false), new Byte(0));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0x01)));
    }

    [Test]
    public void COUNTER_Byte_Load_OnlyOnTick()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Byte>(clock);

        clock.Set(new Bit(false));
        counter.EVal(Bit.One, new Byte(0xAA));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0)));

        clock.Set(Bit.One);
        counter.EVal(Bit.One, new Byte(0xAA));
        clock.Set(new Bit(false));
        counter.EVal(Bit.One, new Byte(0xAA));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xAA)));

        clock.Set(new Bit(false));
        counter.EVal(Bit.One, new Byte(0xBB));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xAA)));

        clock.Set(Bit.One);
        counter.EVal(Bit.One, new Byte(0xBB));
        clock.Set(new Bit(false));
        counter.EVal(Bit.One, new Byte(0xBB));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xBB)));
    }

    [Test]
    public void COUNTER_Byte_Reset_ResetsToZero()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Byte>(clock);

        for (int i = 1; i <= 5; i++)
            CycleCounter(counter, new Bit(false), new Byte(0));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(5)));

        counter.Reset();
        // Force a cycle to latch the reset value (0)
        CycleCounter(counter, Bit.One, new Byte(0));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0)));

        CycleCounter(counter, new Bit(false), new Byte(0));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(1)));
    }

    [Test]
    public void COUNTER_Byte_WrapsAroundCorrectly()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Byte>(clock);
        CycleCounter(counter, Bit.One, new Byte(0xFF));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xFF)));

        CycleCounter(counter, new Bit(false), new Byte(0));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0x00)));

        CycleCounter(counter, new Bit(false), new Byte(0));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0x01)));
    }

    [Test]
    public void COUNTER_NoTick_DoesNotChangeState()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Byte>(clock);
        CycleCounter(counter, Bit.One, new Byte(0xAA));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xAA)));

        clock.Set(new Bit(false));
        counter.EVal(Bit.One, new Byte(0x55));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xAA)));

        clock.Set(new Bit(false));
        counter.EVal(new Bit(false), new Byte(0));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xAA)));

        // Full cycle to update
        clock.Set(Bit.One);
        counter.EVal(Bit.One, new Byte(0x55));
        clock.Set(new Bit(false));
        counter.EVal(Bit.One, new Byte(0x55));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0x55)));

        clock.Set(new Bit(false));
        counter.EVal(new Bit(false), new Byte(0));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0x55)));
    }

    [Test]
    public void COUNTER_Byte_AllStates_Sampled_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 1000; i++)
        {
            int init = random.Next(0, 256);
            int load = random.Next(0, 2);
            int loadValue = random.Next(0, 256);
            int tick = random.Next(0, 2);

            var clock = new CLOCK();
            var counter = new COUNTER<Byte>(clock);
            // Load initial value via a cycle
            CycleCounter(counter, Bit.One, new Byte(init));
            Assert.That((Byte)counter, Is.EqualTo(new Byte(init))); // sanity

            Byte expected;
            if (tick == 0)
            {
                expected = new Byte(init);
                // No clock edge – just call EVal (does nothing)
                counter.EVal(new Bit(load), new Byte(loadValue));
            }
            else
            {
                if (load == 1)
                    expected = new Byte(loadValue);
                else
                    expected = new Byte((byte)(init + 1));

                CycleCounter(counter, new Bit(load), new Byte(loadValue));
            }

            Byte actual = (Byte)counter;
            Assert.That(actual, Is.EqualTo(expected),
                $"Init={init:X2}, Load={load}, LoadValue={loadValue:X2}, Tick={tick}");
        }
    }

    // ==========================================
    // SHORT COUNTER TESTS
    // ==========================================

    [Test]
    public void COUNTER_Short_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            var clock = new CLOCK();
            var counter = new COUNTER<Short>(clock);
            ushort expected = 0;

            int steps = random.Next(1, 20);
            for (int step = 0; step < steps; step++)
            {
                bool loadFlag = random.Next(0, 2) == 1;
                ushort loadValue = (ushort)random.Next(0x0000, 0xFFFF + 1);

                CycleCounter(counter, new Bit(loadFlag ? 1 : 0), new Short(loadValue));

                if (loadFlag)
                    expected = loadValue;
                else
                    expected++;

                Assert.That((Short)counter, Is.EqualTo(new Short(expected)),
                    $"Step {step}: load={loadFlag}, expected={expected:X4}");
            }
        }
    }

    [Test]
    public void COUNTER_Short_ConstructorWithInitialValue_StartsAtValue()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Short>(clock);
        CycleCounter(counter, Bit.One, new Short(0xAAAA));
        Assert.That((Short)counter, Is.EqualTo(new Short(0xAAAA)));

        CycleCounter(counter, new Bit(false), new Short(0));
        Assert.That((Short)counter, Is.EqualTo(new Short(0xAAAB)));

        CycleCounter(counter, Bit.One, new Short(0x5555));
        Assert.That((Short)counter, Is.EqualTo(new Short(0x5555)));
    }

    [Test]
    public void COUNTER_Short_WrapsAroundCorrectly()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Short>(clock);
        CycleCounter(counter, Bit.One, new Short(0xFFFF));
        Assert.That((Short)counter, Is.EqualTo(new Short(0xFFFF)));

        CycleCounter(counter, new Bit(false), new Short(0));
        Assert.That((Short)counter, Is.EqualTo(new Short(0x0000)));

        CycleCounter(counter, new Bit(false), new Short(0));
        Assert.That((Short)counter, Is.EqualTo(new Short(0x0001)));
    }

    // ==========================================
    // INT COUNTER TESTS
    // ==========================================

    [Test]
    public void COUNTER_Int_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            var clock = new CLOCK();
            var counter = new COUNTER<Int>(clock);
            uint expected = 0;

            int steps = random.Next(1, 20);
            for (int step = 0; step < steps; step++)
            {
                bool loadFlag = random.Next(0, 2) == 1;
                uint loadValue = (uint)random.Next();

                CycleCounter(counter, new Bit(loadFlag ? 1 : 0), new Int(loadValue));

                if (loadFlag)
                    expected = loadValue;
                else
                    expected++;

                Assert.That((Int)counter, Is.EqualTo(new Int(expected)),
                    $"Step {step}: load={loadFlag}, expected={expected:X8}");
            }
        }
    }

    [Test]
    public void COUNTER_Int_ConstructorWithInitialValue_StartsAtValue()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Int>(clock);
        CycleCounter(counter, Bit.One, new Int(0xAAAAAAAA));
        Assert.That((Int)counter, Is.EqualTo(new Int(0xAAAAAAAA)));

        CycleCounter(counter, new Bit(false), new Int(0));
        Assert.That((Int)counter, Is.EqualTo(new Int(0xAAAAAAAB)));

        CycleCounter(counter, Bit.One, new Int(0x55555555));
        Assert.That((Int)counter, Is.EqualTo(new Int(0x55555555)));
    }

    [Test]
    public void COUNTER_Int_WrapsAroundCorrectly()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Int>(clock);
        CycleCounter(counter, Bit.One, new Int(0xFFFFFFFF));
        Assert.That((Int)counter, Is.EqualTo(new Int(0xFFFFFFFF)));

        CycleCounter(counter, new Bit(false), new Int(0));
        Assert.That((Int)counter, Is.EqualTo(new Int(0x00000000)));

        CycleCounter(counter, new Bit(false), new Int(0));
        Assert.That((Int)counter, Is.EqualTo(new Int(0x00000001)));
    }

    // ==========================================
    // LONG COUNTER TESTS
    // ==========================================

    [Test]
    public void COUNTER_Long_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            var clock = new CLOCK();
            var counter = new COUNTER<Long>(clock);
            ulong expected = 0;

            int steps = random.Next(1, 20);
            for (int step = 0; step < steps; step++)
            {
                bool loadFlag = random.Next(0, 2) == 1;
                ulong loadValue = ((ulong)random.Next() << 32) | (uint)random.Next();

                CycleCounter(counter, new Bit(loadFlag ? 1 : 0), new Long(loadValue));

                if (loadFlag)
                    expected = loadValue;
                else
                    expected++;

                Assert.That((Long)counter, Is.EqualTo(new Long(expected)),
                    $"Step {step}: load={loadFlag}, expected={expected:X16}");
            }
        }
    }

    [Test]
    public void COUNTER_Long_ConstructorWithInitialValue_StartsAtValue()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Long>(clock);
        CycleCounter(counter, Bit.One, new Long(0xAAAAAAAAAAAAAAAAL));
        Assert.That((Long)counter, Is.EqualTo(new Long(0xAAAAAAAAAAAAAAAAL)));

        CycleCounter(counter, new Bit(false), new Long(0));
        Assert.That((Long)counter, Is.EqualTo(new Long(0xAAAAAAAAAAAAAAABL)));

        CycleCounter(counter, Bit.One, new Long(0x5555555555555555L));
        Assert.That((Long)counter, Is.EqualTo(new Long(0x5555555555555555L)));
    }

    [Test]
    public void COUNTER_Long_WrapsAroundCorrectly()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Long>(clock);
        CycleCounter(counter, Bit.One, new Long(0xFFFFFFFFFFFFFFFFL));
        Assert.That((Long)counter, Is.EqualTo(new Long(0xFFFFFFFFFFFFFFFFL)));

        CycleCounter(counter, new Bit(false), new Long(0));
        Assert.That((Long)counter, Is.EqualTo(new Long(0x0000000000000000L)));

        CycleCounter(counter, new Bit(false), new Long(0));
        Assert.That((Long)counter, Is.EqualTo(new Long(0x0000000000000001L)));
    }

    // ==========================================
    // RESET TESTS
    // ==========================================

    [Test]
    public void COUNTER_Reset_ResetsToZero()
    {
        var clock = new CLOCK();
        var counter = new COUNTER<Byte>(clock);

        for (int i = 1; i <= 10; i++)
            CycleCounter(counter, new Bit(false), new Byte(0));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(10)));

        counter.Reset();
        // Force a cycle to latch the reset value (0)
        CycleCounter(counter, Bit.One, new Byte(0));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0)));

        CycleCounter(counter, new Bit(false), new Byte(0));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(1)));
    }

    // ==========================================
    // TYPE PROMOTION TESTS
    // ==========================================

    [Test]
    public void COUNTER_MixedTypes_CompilesAndWorks()
    {
        var clock = new CLOCK();
        var counter1 = new COUNTER<Byte>(clock);

        CycleCounter(counter1, Bit.One, Byte.One);
        Byte actual1 = (Byte)counter1;
        Assert.That(actual1, Is.EqualTo(new Byte(0x01)));

        var counter2 = new COUNTER<Short>(clock);
        CycleCounter(counter2, Bit.One, new Byte(0xAA));
        Short actual2 = (Short)counter2;
        Assert.That(actual2, Is.EqualTo(new Short(0x00AA)));

        var counter3 = new COUNTER<Int>(clock);
        CycleCounter(counter3, Bit.One, new Short(0xAAAA));
        Int actual3 = (Int)counter3;
        Assert.That(actual3, Is.EqualTo(new Int(0x0000AAAA)));

        var counter4 = new COUNTER<Long>(clock);
        CycleCounter(counter4, Bit.One, new Int(0xAAAAAAAA));
        Long actual4 = (Long)counter4;
        Assert.That(actual4, Is.EqualTo(new Long(0x00000000AAAAAAAA)));
    }
}