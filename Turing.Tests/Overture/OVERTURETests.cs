using Turing.Core.Overture;

namespace Turing.Tests.Overture;

[TestFixture]
internal class OVERTURETests
{
    private OVERTURE _cpu;

    [SetUp]
    public void Setup()
    {
        _cpu = new OVERTURE();
    }

    // ==========================================
    // REGISTER MOVE TESTS (MOVE mode: bit6=0, bit7=1 -> [..., 0, 1])
    // ==========================================

    [Test]
    public void Move_RegisterToRegister_Works()
    {
        // Load 0xAA into Reg0: src=6 (0,1,1), dst=0 (0,0,0), mode=MOVE (1, 0) -> [0,1,1, 0,0,0, 0,1]
        _cpu.EVal([1, 0, 0, 0, 0, 1, 1, 0], new Byte(0xAA));
        // Move Reg0 -> Reg1: src=0 (0,0,0), dst=1 (1,0,0), mode=MOVE (1, 0) -> [0,0,0, 1,0,0, 0,1]
        _cpu.EVal([1, 0, 0, 0, 1, 0, 0, 0], new Byte(0x00));
        // Move Reg1 -> Output: src=1 (1,0,0), dst=6 (0,1,1), mode=MOVE (1, 0) -> [1,0,0, 0,1,1, 0,1]
        _cpu.EVal([1, 0, 1, 1, 0, 0, 0, 1], new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0xAA)));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(3));
    }

    [Test]
    public void Move_InputToRegister_Works()
    {
        // Load input (src=6: 0,1,1) to Reg2 (dst=2: 0,1,0), mode=MOVE (1, 0) -> [0,1,1, 0,1,0, 0,1]
        _cpu.EVal([0, 1, 1, 0, 1, 0, 0, 1], new Byte(0x55));
        // Move Reg2 -> Output: src=2 (0,1,0), dst=6 (0,1,1), mode=MOVE (1, 0) -> [0,1,0, 0,1,1, 0,1]
        _cpu.EVal([0, 1, 0, 0, 1, 1, 0, 1], new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x55)));
    }

    [Test]
    public void Move_RegisterToOutput_Works()
    {
        // Load input to Reg3: src=6 (0,1,1), dst=3 (1,1,0), mode=MOVE (1, 0) -> [0,1,1, 1,1,0, 0,1]
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0xCC));
        // Move Reg3 -> Output: src=3 (1,1,0), dst=6 (0,1,1), mode=MOVE (1, 0) -> [1,1,0, 0,1,1, 0,1]
        _cpu.EVal([1, 1, 0, 0, 1, 1, 0, 1], new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0xCC)));
    }

    [Test]
    public void Move_InputToOutput_Works()
    {
        // Move input (src=6: 0,1,1) to Output (dst=6: 0,1,1), mode=MOVE (1, 0) -> [0,1,1, 0,1,1, 0,1]
        _cpu.EVal([0, 1, 1, 0, 1, 1, 0, 1], new Byte(0x77));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x77)));
    }

    [Test]
    public void WriteOnlyOnTick_Works()
    {
        // Load Reg0: src=6, dst=0, mode=MOVE -> [0,1,1, 0,0,0, 0,1]
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0xAA));
        // Move Reg0 -> Output: src=0, dst=6, mode=MOVE -> [0,0,0, 0,1,1, 0,1]
        _cpu.EVal([0, 0, 0, 0, 1, 1, 0, 1], new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0xAA)));
    }

    [Test]
    public void MultipleMoves_Work()
    {
        // Load 0x01 into Reg0
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x01));
        // Move Reg0 -> Reg1: src=0, dst=1 -> [0,0,0, 1,0,0, 0,1]
        _cpu.EVal([0, 0, 0, 1, 0, 0, 0, 1], new Byte(0x00));
        // Move Reg1 -> Reg2: src=1, dst=2 -> [1,0,0, 0,1,0, 0,1]
        _cpu.EVal([1, 0, 0, 0, 1, 0, 0, 1], new Byte(0x00));
        // Move Reg2 -> Output: src=2, dst=6 -> [0,1,0, 0,1,1, 0,1]
        _cpu.EVal([0, 1, 0, 0, 1, 1, 0, 1], new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x01)));
    }

    // ==========================================
    // ALU PHASE TESTS (ALU mode: bit6=1, bit7=0 -> [..., 1, 0])
    // ==========================================

    [Test]
    public void ALU_AddsReg1AndReg2_StoresInReg3()
    {
        // Load 0x0A into Reg1 (MOVE mode)
        _cpu.EVal([0, 1, 1, 1, 0, 0, 0, 1], new Byte(0x0A));
        // Load 0x05 into Reg2 (MOVE mode)
        _cpu.EVal([0, 1, 1, 0, 1, 0, 0, 1], new Byte(0x05));
        // ALU ADD (opcode=4: 0,0,1, mode=ALU: 1,0) -> [0,0,1, 0,0,0, 1,0]
        _cpu.EVal([0, 0, 1, 0, 0, 0, 1, 0], new Byte(0x00));
        // Move Reg3 -> Output (MOVE mode)
        _cpu.EVal([1, 1, 0, 0, 1, 1, 0, 1], new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x0F)));
    }

    [Test]
    public void ALU_SubReg1MinusReg2_StoresInReg3()
    {
        _cpu.EVal([0, 1, 1, 1, 0, 0, 0, 1], new Byte(0x0A));
        _cpu.EVal([0, 1, 1, 0, 1, 0, 0, 1], new Byte(0x03));
        // ALU SUB (opcode=5: 1,0,1, mode=ALU: 1,0) -> [1,0,1, 0,0,0, 1,0]
        _cpu.EVal([1, 0, 1, 0, 0, 0, 1, 0], new Byte(0x00));
        _cpu.EVal([1, 1, 0, 0, 1, 1, 0, 1], new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x07)));
    }

    [Test]
    public void ALU_AndReg1AndReg2_StoresInReg3()
    {
        _cpu.EVal([0, 1, 1, 1, 0, 0, 0, 1], new Byte(0x0F));
        _cpu.EVal([0, 1, 1, 0, 1, 0, 0, 1], new Byte(0x33));
        // ALU AND (opcode=2: 0,1,0, mode=ALU: 1,0) -> [0,1,0, 0,0,0, 1,0]
        _cpu.EVal([0, 1, 0, 0, 0, 0, 1, 0], new Byte(0x00));
        _cpu.EVal([1, 1, 0, 0, 1, 1, 0, 1], new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x03)));
    }

    [Test]
    public void ALU_DoesNotAffectOtherRegisters()
    {
        // Load Reg0 with 0x55 (MOVE mode)
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x55));
        // ALU ADD (ALU mode)
        _cpu.EVal([0, 0, 1, 0, 0, 0, 1, 0], new Byte(0x00));
        // Move Reg0 -> Output (MOVE mode)
        _cpu.EVal([0, 0, 0, 0, 1, 1, 0, 1], new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x55)));
    }

    [Test]
    public void ALU_DoesNotOutput_WhenInALU()
    {
        _cpu.EVal([0, 1, 1, 1, 0, 0, 0, 1], new Byte(0x05));
        _cpu.EVal([0, 1, 1, 0, 1, 0, 0, 1], new Byte(0x03));
        _cpu.EVal([0, 0, 1, 0, 0, 0, 1, 0], new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x00)));
    }

    // ==========================================
    // IMMEDIATE PHASE TESTS (IMM mode: bit6=0, bit7=0 -> [..., 0, 0])
    // ==========================================

    [Test]
    public void Immediate_StoresLower6BitsIntoReg0()
    {
        // IMM mode: bit6=0, bit7=0. Value 0x2A -> [0,1,0,1,0,1, 0,0]
        _cpu.EVal([0, 1, 0, 1, 0, 1, 0, 0], new Byte(0x00));
        // Move Reg0 -> Output (MOVE mode: 0,1)
        _cpu.EVal([0, 0, 0, 0, 1, 1, 0, 1], new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x2A)));
    }

    [Test]
    public void Immediate_IgnoresUpperBits()
    {
        _cpu.EVal([1, 1, 1, 1, 1, 1, 0, 0], new Byte(0x00));
        _cpu.EVal([0, 0, 0, 0, 1, 1, 0, 1], new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x3F)));
    }

    [Test]
    public void Immediate_DoesNotAffectOtherRegisters()
    {
        // Load Reg1 with 0xAA (MOVE mode)
        _cpu.EVal([0, 1, 1, 1, 0, 0, 0, 1], new Byte(0xAA));
        // Immediate 0x12 to Reg0 (IMM mode)
        _cpu.EVal([0, 1, 0, 0, 1, 0, 0, 0], new Byte(0x00));
        // Move Reg1 -> Output (MOVE mode)
        _cpu.EVal([1, 0, 0, 0, 1, 1, 0, 1], new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0xAA)));
    }

    // ==========================================
    // CONDITION PHASE TESTS (COND mode: bit6=1, bit7=1 -> [..., 1, 1])
    // ==========================================

    [Test]
    public void Condition_EqualsZero_True()
    {
        // Reg3 = 0 (MOVE mode)
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0x00));
        // Reg0 = 0x42 (MOVE mode)
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        // COND == 0 (code 2: 0,1,0, mode=COND: 1,1) -> [0,1,0, 0,0,0, 1,1]
        _cpu.EVal([0, 1, 0, 0, 0, 0, 1, 1], new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_EqualsZero_False()
    {
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0x05));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal([0, 1, 0, 0, 0, 0, 1, 1], new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_Never_DoesNotJump()
    {
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0x00));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal([0, 0, 0, 0, 0, 0, 1, 1], new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_Always_Jumps()
    {
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        _cpu.EVal([1, 0, 0, 0, 0, 0, 1, 1], new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_NotEqualsZero_True()
    {
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0x05));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        _cpu.EVal([1, 1, 0, 0, 0, 0, 1, 1], new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_NotEqualsZero_False()
    {
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0x00));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal([1, 1, 0, 0, 0, 0, 1, 1], new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_LessThanZero_True()
    {
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0xFB));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        _cpu.EVal([0, 0, 1, 0, 0, 0, 1, 1], new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_LessThanZero_False()
    {
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0x05));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal([0, 0, 1, 0, 0, 0, 1, 1], new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_GreaterThanZero_True()
    {
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0x05));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        _cpu.EVal([1, 1, 1, 0, 0, 0, 1, 1], new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_GreaterThanZero_False()
    {
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0xFB));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal([1, 1, 1, 0, 0, 0, 1, 1], new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_GeZero_True()
    {
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0x00));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        _cpu.EVal([1, 0, 1, 0, 0, 0, 1, 1], new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));

        _cpu.Reset();
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0x05));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        _cpu.EVal([1, 0, 1, 0, 0, 0, 1, 1], new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_GeZero_False()
    {
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0xFB));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal([1, 0, 1, 0, 0, 0, 1, 1], new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_LeZero_True()
    {
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0x00));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 1, 1], new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));

        _cpu.Reset();
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0xFB));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 1, 1], new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_LeZero_False()
    {
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0x05));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal([0, 1, 1, 0, 0, 0, 1, 1], new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_DoesNotAffectRegisters()
    {
        _cpu.EVal([0, 1, 1, 1, 1, 0, 0, 1], new Byte(0x05));
        _cpu.EVal([0, 1, 1, 0, 0, 0, 0, 1], new Byte(0x42));
        _cpu.EVal([1, 1, 1, 0, 0, 0, 1, 1], new Byte(0x00)); // >0, jumps
        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x00)));

        // Move Reg0 -> Output
        _cpu.EVal([0, 0, 0, 0, 1, 1, 0, 1], new Byte(0x00));
        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x42)));
    }
}