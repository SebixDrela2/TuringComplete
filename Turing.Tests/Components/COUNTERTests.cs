using Turing.Core.Components.Memory;

namespace Turing.Tests.Components.Memory;

[TestFixture]
internal class COUNTERTests
{
    // ==========================================
    // BIT COUNTER TESTS - Exhaustive
    // ==========================================

    [Test]
    public void COUNTER_Bit_CountsCorrectly()
    {
        var counter = new COUNTER<Bit>();

        Assert.That((Bit)counter, Is.EqualTo(new Bit(0)));

        counter.EVal(new Bit(false), new Bit(0), new Bit(true));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(1)));

        counter.EVal(new Bit(false), new Bit(0), new Bit(true));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(0)));

        counter.EVal(new Bit(false), new Bit(0), new Bit(true));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(1)));
    }

    [Test]
    public void COUNTER_Bit_Load_LoadsValue()
    {
        var counter = new COUNTER<Bit>();

        counter.EVal(new Bit(true), new Bit(1), new Bit(true));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(1)));

        counter.EVal(new Bit(false), new Bit(0), new Bit(true));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(0)));

        counter.EVal(new Bit(true), new Bit(1), new Bit(true));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(1)));
    }

    [Test]
    public void COUNTER_Bit_Load_OnlyOnTick()
    {
        var counter = new COUNTER<Bit>();

        counter.EVal(new Bit(true), new Bit(1), new Bit(false));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(0)));

        counter.EVal(new Bit(true), new Bit(1), new Bit(true));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(1)));

        counter.EVal(new Bit(true), new Bit(0), new Bit(false));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(1)));
    }

    [Test]
    public void COUNTER_Bit_Reset_ResetsToZero()
    {
        var counter = new COUNTER<Bit>();
        counter.EVal(new Bit(true), new Bit(1), new Bit(true));
        Assert.That((Bit)counter, Is.EqualTo(new Bit(1)));

        counter.Reset();
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
                        var counter = new COUNTER<Bit>(new Bit(init));
                        Bit expected;

                        if (tick == 0)
                        {
                            expected = new Bit(init);
                        }
                        else if (load == 1)
                        {
                            expected = new Bit(loadValue);
                        }
                        else
                        {
                            expected = new Bit(init == 0 ? 1 : 0);
                        }

                        counter.EVal(new Bit(load), new Bit(loadValue), new Bit(tick));
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
        var counter = new COUNTER<Byte>();

        Assert.That((Byte)counter, Is.EqualTo(new Byte(0)));

        for (int i = 1; i <= 10; i++)
        {
            counter.EVal(new Bit(false), new Byte(0), new Bit(true));
            Assert.That((Byte)counter, Is.EqualTo(new Byte(i)));
        }
    }

    [Test]
    public void COUNTER_Byte_Load_LoadsValue()
    {
        var counter = new COUNTER<Byte>();

        for (int i = 1; i <= 5; i++)
        {
            counter.EVal(new Bit(false), new Byte(0), new Bit(true));
        }
        Assert.That((Byte)counter, Is.EqualTo(new Byte(5)));

        counter.EVal(new Bit(true), new Byte(0xAA), new Bit(true));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xAA)));

        counter.EVal(new Bit(false), new Byte(0), new Bit(true));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xAB)));

        counter.EVal(new Bit(true), new Byte(0x00), new Bit(true));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0x00)));

        counter.EVal(new Bit(false), new Byte(0), new Bit(true));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0x01)));
    }

    [Test]
    public void COUNTER_Byte_Load_OnlyOnTick()
    {
        var counter = new COUNTER<Byte>();

        counter.EVal(new Bit(true), new Byte(0xAA), new Bit(false));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0)));

        counter.EVal(new Bit(true), new Byte(0xAA), new Bit(true));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xAA)));

        counter.EVal(new Bit(true), new Byte(0xBB), new Bit(false));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xAA)));

        counter.EVal(new Bit(true), new Byte(0xBB), new Bit(true));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xBB)));
    }

    [Test]
    public void COUNTER_Byte_Reset_ResetsToZero()
    {
        var counter = new COUNTER<Byte>();

        for (int i = 1; i <= 5; i++)
        {
            counter.EVal(new Bit(false), new Byte(0), new Bit(true));
        }
        Assert.That((Byte)counter, Is.EqualTo(new Byte(5)));

        counter.Reset();
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0)));

        counter.EVal(new Bit(false), new Byte(0), new Bit(true));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(1)));
    }

    [Test]
    public void COUNTER_Byte_WrapsAroundCorrectly()
    {
        var counter = new COUNTER<Byte>(0xFF);
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xFF)));

        counter.EVal(new Bit(false), new Byte(0), new Bit(true));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0x00)));

        counter.EVal(new Bit(false), new Byte(0), new Bit(true));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0x01)));
    }

    [Test]
    public void COUNTER_NoTick_DoesNotChangeState()
    {
        var counter = new COUNTER<Byte>(0xAA);
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xAA)));

        // Tick low with load
        counter.EVal(new Bit(true), new Byte(0x55), new Bit(false));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xAA)));

        // Tick low without load
        counter.EVal(new Bit(false), new Byte(0), new Bit(false));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0xAA)));

        // Tick high with load
        counter.EVal(new Bit(true), new Byte(0x55), new Bit(true));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0x55)));

        // Tick low - should stay at 0x55
        counter.EVal(new Bit(false), new Byte(0), new Bit(false));
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

            var counter = new COUNTER<Byte>(new Byte(init));
            Byte expected;

            if (tick == 0)
            {
                expected = new Byte(init);
            }
            else if (load == 1)
            {
                expected = new Byte(loadValue);
            }
            else
            {
                expected = new Byte((byte)(init + 1));
            }

            counter.EVal(new Bit(load), new Byte(loadValue), new Bit(tick));
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
            var counter = new COUNTER<Short>();
            ushort expected = 0;

            int steps = random.Next(1, 20);
            for (int step = 0; step < steps; step++)
            {
                bool loadFlag = random.Next(0, 2) == 1;
                ushort loadValue = (ushort)random.Next(0x0000, 0xFFFF + 1);

                if (loadFlag)
                {
                    counter.EVal(new Bit(true), new Short(loadValue), new Bit(true));
                    expected = loadValue;
                }
                else
                {
                    counter.EVal(new Bit(false), new Short(0), new Bit(true));
                    expected++;
                }

                Assert.That((Short)counter, Is.EqualTo(new Short(expected)),
                    $"Step {step}: load={loadFlag}, expected={expected:X4}");
            }
        }
    }

    [Test]
    public void COUNTER_Short_ConstructorWithInitialValue_StartsAtValue()
    {
        var counter = new COUNTER<Short>(0xAAAA);
        Assert.That((Short)counter, Is.EqualTo(new Short(0xAAAA)));

        counter.EVal(new Bit(false), new Short(0), new Bit(true));
        Assert.That((Short)counter, Is.EqualTo(new Short(0xAAAB)));

        counter.EVal(new Bit(true), new Short(0x5555), new Bit(true));
        Assert.That((Short)counter, Is.EqualTo(new Short(0x5555)));
    }

    [Test]
    public void COUNTER_Short_WrapsAroundCorrectly()
    {
        var counter = new COUNTER<Short>(0xFFFF);
        Assert.That((Short)counter, Is.EqualTo(new Short(0xFFFF)));

        counter.EVal(new Bit(false), new Short(0), new Bit(true));
        Assert.That((Short)counter, Is.EqualTo(new Short(0x0000)));

        counter.EVal(new Bit(false), new Short(0), new Bit(true));
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
            var counter = new COUNTER<Int>();
            uint expected = 0;

            int steps = random.Next(1, 20);
            for (int step = 0; step < steps; step++)
            {
                bool loadFlag = random.Next(0, 2) == 1;
                uint loadValue = (uint)random.Next();

                if (loadFlag)
                {
                    counter.EVal(new Bit(true), new Int(loadValue), new Bit(true));
                    expected = loadValue;
                }
                else
                {
                    counter.EVal(new Bit(false), new Int(0), new Bit(true));
                    expected++;
                }

                Assert.That((Int)counter, Is.EqualTo(new Int(expected)),
                    $"Step {step}: load={loadFlag}, expected={expected:X8}");
            }
        }
    }

    [Test]
    public void COUNTER_Int_ConstructorWithInitialValue_StartsAtValue()
    {
        var counter = new COUNTER<Int>(0xAAAAAAAA);
        Assert.That((Int)counter, Is.EqualTo(new Int(0xAAAAAAAA)));

        counter.EVal(new Bit(false), new Int(0), new Bit(true));
        Assert.That((Int)counter, Is.EqualTo(new Int(0xAAAAAAAB)));

        counter.EVal(new Bit(true), new Int(0x55555555), new Bit(true));
        Assert.That((Int)counter, Is.EqualTo(new Int(0x55555555)));
    }

    [Test]
    public void COUNTER_Int_WrapsAroundCorrectly()
    {
        var counter = new COUNTER<Int>(0xFFFFFFFF);
        Assert.That((Int)counter, Is.EqualTo(new Int(0xFFFFFFFF)));

        counter.EVal(new Bit(false), new Int(0), new Bit(true));
        Assert.That((Int)counter, Is.EqualTo(new Int(0x00000000)));

        counter.EVal(new Bit(false), new Int(0), new Bit(true));
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
            var counter = new COUNTER<Long>();
            ulong expected = 0;

            int steps = random.Next(1, 20);
            for (int step = 0; step < steps; step++)
            {
                bool loadFlag = random.Next(0, 2) == 1;
                ulong loadValue = ((ulong)random.Next() << 32) | (uint)random.Next();

                if (loadFlag)
                {
                    counter.EVal(new Bit(true), new Long(loadValue), new Bit(true));
                    expected = loadValue;
                }
                else
                {
                    counter.EVal(new Bit(false), new Long(0), new Bit(true));
                    expected++;
                }

                Assert.That((Long)counter, Is.EqualTo(new Long(expected)),
                    $"Step {step}: load={loadFlag}, expected={expected:X16}");
            }
        }
    }

    [Test]
    public void COUNTER_Long_ConstructorWithInitialValue_StartsAtValue()
    {
        var counter = new COUNTER<Long>(0xAAAAAAAAAAAAAAAAL);
        Assert.That((Long)counter, Is.EqualTo(new Long(0xAAAAAAAAAAAAAAAAL)));

        counter.EVal(new Bit(false), new Long(0), new Bit(true));
        Assert.That((Long)counter, Is.EqualTo(new Long(0xAAAAAAAAAAAAAAABL)));

        counter.EVal(new Bit(true), new Long(0x5555555555555555L), new Bit(true));
        Assert.That((Long)counter, Is.EqualTo(new Long(0x5555555555555555L)));
    }

    [Test]
    public void COUNTER_Long_WrapsAroundCorrectly()
    {
        var counter = new COUNTER<Long>(0xFFFFFFFFFFFFFFFFL);
        Assert.That((Long)counter, Is.EqualTo(new Long(0xFFFFFFFFFFFFFFFFL)));

        counter.EVal(new Bit(false), new Long(0), new Bit(true));
        Assert.That((Long)counter, Is.EqualTo(new Long(0x0000000000000000L)));

        counter.EVal(new Bit(false), new Long(0), new Bit(true));
        Assert.That((Long)counter, Is.EqualTo(new Long(0x0000000000000001L)));
    }

    // ==========================================
    // RESET TESTS
    // ==========================================

    [Test]
    public void COUNTER_Reset_ResetsToZero()
    {
        var counter = new COUNTER<Byte>();

        for (int i = 1; i <= 10; i++)
        {
            counter.EVal(new Bit(false), new Byte(0), new Bit(true));
        }
        Assert.That((Byte)counter, Is.EqualTo(new Byte(10)));

        counter.Reset();
        Assert.That((Byte)counter, Is.EqualTo(new Byte(0)));

        counter.EVal(new Bit(false), new Byte(0), new Bit(true));
        Assert.That((Byte)counter, Is.EqualTo(new Byte(1)));
    }

    // ==========================================
    // TYPE PROMOTION TESTS
    // ==========================================

    [Test]
    public void COUNTER_MixedTypes_CompilesAndWorks()
    {
        var counter1 = new COUNTER<Byte>();
        counter1.EVal(new Bit(true), new Bit(true), new Bit(true));
        Byte actual1 = (Byte)counter1;
        Assert.That(actual1, Is.EqualTo(new Byte(0x01)));

        var counter2 = new COUNTER<Short>();
        counter2.EVal(new Bit(true), new Byte(0xAA), new Bit(true));
        Short actual2 = (Short)counter2;
        Assert.That(actual2, Is.EqualTo(new Short(0x00AA)));

        var counter3 = new COUNTER<Int>();
        counter3.EVal(new Bit(true), new Short(0xAAAA), new Bit(true));
        Int actual3 = (Int)counter3;
        Assert.That(actual3, Is.EqualTo(new Int(0x0000AAAA)));

        var counter4 = new COUNTER<Long>();
        counter4.EVal(new Bit(true), new Int(0xAAAAAAAA), new Bit(true));
        Long actual4 = (Long)counter4;
        Assert.That(actual4, Is.EqualTo(new Long(0x00000000AAAAAAAA)));
    }
}