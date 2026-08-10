using Turing.Core.Components.Memory;

namespace Turing.Tests.Components.Memory;

[TestFixture]
internal class REGISTERTests
{
    [Test]
    public void REGISTER_Bit_Sequence_ReturnsCorrectOutput()
    {
        int[] saves = [1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1];
        int[] values = [1, 1, 0, 0, 0, 1, 0, 1, 1, 1, 0, 0, 0];

        var register = new REGISTER<Bit>();
        // Initialize register to 0 (default state)
        // No need to explicitly initialize, but we'll track reference state
        Bit refLatch = new Bit(0);
        Bit refDelay = new Bit(0);

        for (int i = 0; i < saves.Length; i++)
        {
            Bit save = new Bit(saves[i]);
            Bit value = new Bit(values[i]);
            Bit tick = new Bit(i & 1);

            // Update reference model
            if (save.Value) refLatch = value;
            if (tick.Value) refDelay = refLatch;
            Bit expected = refDelay;

            // Act
            register.EVal(save, value, tick);
            Bit actual = (Bit)register;

            // Assert
            Assert.That(actual, Is.EqualTo(expected),
                $"Step {i}: save={saves[i]}, value={values[i]}, tick={i & 1}");
        }
    }

    // ==========================================
    // BYTE TESTS - Specific Sequence (Now Dynamic)
    // ==========================================

    [Test]
    public void REGISTER_Byte_Sequence_ReturnsCorrectOutput()
    {
        byte v = 0b10010111;
        int[] saves = [1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1];
        int[] values = [v, v, 0, 0, 0, v, 0, v, v, v, 0, 0, 0];

        var register = new REGISTER<Byte>();
        Byte refLatch = new Byte(0);
        Byte refDelay = new Byte(0);

        for (int i = 0; i < saves.Length; i++)
        {
            Bit save = new Bit(saves[i]);
            Byte value = new Byte(values[i]);
            Bit tick = new Bit(i & 1);

            if (save.Value) refLatch = value;
            if (tick.Value) refDelay = refLatch;
            Byte expected = refDelay;

            register.EVal(save, value, tick);
            Byte actual = (Byte)register;

            Assert.That(actual, Is.EqualTo(expected),
                $"Step {i}: save={saves[i]}, value={values[i]:X2}, tick={i & 1}");
        }
    }

    // ==========================================
    // BIT TESTS - Exhaustive (2x2x2x2 = 16 cases)
    // ==========================================

    [Test]
    public void REGISTER_Bit_Exhaustive_ReturnsCorrectOutput()
    {
        for (int init = 0; init <= 1; init++)
        {
            for (int save = 0; save <= 1; save++)
            {
                for (int value = 0; value <= 1; value++)
                {
                    for (int tick = 0; tick <= 1; tick++)
                    {
                        // Reference model with initial state
                        Bit refLatch = new Bit(init);
                        Bit refDelay = new Bit(init);

                        if (save == 1) refLatch = new Bit(value);
                        if (tick == 1) refDelay = refLatch;

                        var register = new REGISTER<Bit>();
                        // Initialize actual register to init state
                        register.EVal(new Bit(1), new Bit(init), new Bit(1));
                        register.EVal(new Bit(0), new Bit(init), new Bit(0));

                        register.EVal(new Bit(save), new Bit(value), new Bit(tick));
                        Bit actual = (Bit)register;
                        Bit expected = refDelay;

                        Assert.That(actual, Is.EqualTo(expected),
                            $"Init={init}, Save={save}, Value={value}, Tick={tick}");
                    }
                }
            }
        }
    }

    // ==========================================
    // BYTE TESTS - Exhaustive (256x2x256x2)
    // ==========================================

    [Test]
    public void REGISTER_Byte_Exhaustive_ReturnsCorrectOutput()
    {
        for (int init = 0; init < 256; init++)
        {
            for (int save = 0; save <= 1; save++)
            {
                for (int value = 0; value < 256; value++)
                {
                    for (int tick = 0; tick <= 1; tick++)
                    {
                        Byte refLatch = new Byte(init);
                        Byte refDelay = new Byte(init);

                        if (save == 1) refLatch = new Byte(value);
                        if (tick == 1) refDelay = refLatch;

                        var register = new REGISTER<Byte>();
                        register.EVal(new Bit(1), new Byte(init), new Bit(1));
                        register.EVal(new Bit(0), new Byte(init), new Bit(0));

                        register.EVal(new Bit(save), new Byte(value), new Bit(tick));
                        Byte actual = (Byte)register;
                        Byte expected = refDelay;

                        Assert.That(actual, Is.EqualTo(expected),
                            $"Init={init:X2}, Save={save}, Value={value:X2}, Tick={tick}");
                    }
                }
            }
        }
    }

    // ==========================================
    // SHORT TESTS - Random Cases
    // ==========================================

    [Test]
    public void REGISTER_Short_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            ushort init = (ushort)random.Next(0x0000, 0xFFFF + 1);
            ushort value = (ushort)random.Next(0x0000, 0xFFFF + 1);
            int save = random.Next(0, 2);
            int tick = random.Next(0, 2);

            Short refLatch = new Short(init);
            Short refDelay = new Short(init);

            if (save == 1) refLatch = new Short(value);
            if (tick == 1) refDelay = refLatch;

            var register = new REGISTER<Short>();
            register.EVal(new Bit(1), new Short(init), new Bit(1));
            register.EVal(new Bit(0), new Short(init), new Bit(0));

            register.EVal(new Bit(save), new Short(value), new Bit(tick));
            Short actual = (Short)register;
            Short expected = refDelay;

            Assert.That(actual, Is.EqualTo(expected),
                $"Init={init:X4}, Save={save}, Value={value:X4}, Tick={tick}");
        }
    }

    // ==========================================
    // INT TESTS - Random Cases
    // ==========================================

    [Test]
    public void REGISTER_Int_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            uint init = (uint)random.Next();
            uint value = (uint)random.Next();
            int save = random.Next(0, 2);
            int tick = random.Next(0, 2);

            Int refLatch = new Int(init);
            Int refDelay = new Int(init);

            if (save == 1) refLatch = new Int(value);
            if (tick == 1) refDelay = refLatch;

            var register = new REGISTER<Int>();
            register.EVal(new Bit(1), new Int(init), new Bit(1));
            register.EVal(new Bit(0), new Int(init), new Bit(0));

            register.EVal(new Bit(save), new Int(value), new Bit(tick));
            Int actual = (Int)register;
            Int expected = refDelay;

            Assert.That(actual, Is.EqualTo(expected),
                $"Init={init:X8}, Save={save}, Value={value:X8}, Tick={tick}");
        }
    }

    // ==========================================
    // LONG TESTS - Random Cases
    // ==========================================

    [Test]
    public void REGISTER_Long_RandomCases_ReturnsCorrectOutput()
    {
        var random = new Random(42);
        for (int i = 0; i < 100; i++)
        {
            ulong init = ((ulong)random.Next() << 32) | (uint)random.Next();
            ulong value = ((ulong)random.Next() << 32) | (uint)random.Next();
            int save = random.Next(0, 2);
            int tick = random.Next(0, 2);

            Long refLatch = new Long(init);
            Long refDelay = new Long(init);

            if (save == 1) refLatch = new Long(value);
            if (tick == 1) refDelay = refLatch;

            var register = new REGISTER<Long>();
            register.EVal(new Bit(1), new Long(init), new Bit(1));
            register.EVal(new Bit(0), new Long(init), new Bit(0));

            register.EVal(new Bit(save), new Long(value), new Bit(tick));
            Long actual = (Long)register;
            Long expected = refDelay;

            Assert.That(actual, Is.EqualTo(expected),
                $"Init={init:X16}, Save={save}, Value={value:X16}, Tick={tick}");
        }
    }

    // ==========================================
    // RESET TESTS
    // ==========================================

    [Test]
    public void REGISTER_Reset_ResetsStateToZero()
    {
        var register = new REGISTER<Byte>();
        register.EVal(new Bit(1), new Byte(0xAA), new Bit(1));
        register.EVal(new Bit(0), new Byte(0xAA), new Bit(0));
        Assert.That((Byte)register, Is.EqualTo(new Byte(0xAA)));

        register.Reset();
        Assert.That((Byte)register, Is.EqualTo(new Byte(0x00)));
    }

    [Test]
    public void REGISTER_ConstructorWithInitialValue_SetsState()
    {
        var register = new REGISTER<Byte>(0x55);
        Assert.That((Byte)register, Is.EqualTo(new Byte(0x55)));

        register.EVal(new Bit(1), new Byte(0xAA), new Bit(1));
        Assert.That((Byte)register, Is.EqualTo(new Byte(0xAA)));
    }

    // ==========================================
    // MIXED TYPE PROMOTION TESTS
    // ==========================================

    [Test]
    public void REGISTER_MixedTypes_CompilesAndWorks()
    {
        var reg1 = new REGISTER<Byte>();
        reg1.EVal(new Bit(true), new Bit(true), new Bit(true));
        Byte actual1 = (Byte)reg1;
        Assert.That(actual1, Is.EqualTo(new Byte(0x01)));

        var reg2 = new REGISTER<Short>();
        reg2.EVal(new Bit(true), new Byte(0xAA), new Bit(true));
        Short actual2 = (Short)reg2;
        Assert.That(actual2, Is.EqualTo(new Short(0x00AA)));

        var reg3 = new REGISTER<Int>();
        reg3.EVal(new Bit(true), new Short(0xAAAA), new Bit(true));
        Int actual3 = (Int)reg3;
        Assert.That(actual3, Is.EqualTo(new Int(0x0000AAAA)));

        var reg4 = new REGISTER<Long>();
        reg4.EVal(new Bit(true), new Int(0xAAAAAAAA), new Bit(true));
        Long actual4 = (Long)reg4;
        Assert.That(actual4, Is.EqualTo(new Long(0x00000000AAAAAAAA)));
    }
}