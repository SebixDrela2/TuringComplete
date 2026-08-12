using Turing.Core.Overture;
using Turing.Core.Electricity;

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

    private Byte MakeInstruction(int src, int dst, int mode)
    {
        int value = src | (dst << 3) | (mode << 6);
        return new Byte(value);
    }

    // ==========================================
    // REGISTER PHASE TESTS (Move mode)
    // ==========================================

    [Test]
    public void Move_RegisterToRegister_Works()
    {
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0xAA));
        _cpu.EVal(MakeInstruction(0, 1, 2), new Byte(0x00));
        _cpu.EVal(MakeInstruction(1, 6, 2), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0xAA)));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(3));
    }

    [Test]
    public void Move_InputToRegister_Works()
    {
        _cpu.EVal(MakeInstruction(6, 2, 2), new Byte(0x55));
        _cpu.EVal(MakeInstruction(2, 6, 2), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0x55)));
    }

    [Test]
    public void Move_RegisterToOutput_Works()
    {
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0xCC));
        _cpu.EVal(MakeInstruction(3, 6, 2), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0xCC)));
    }

    [Test]
    public void Move_InputToOutput_Works()
    {
        _cpu.EVal(MakeInstruction(6, 6, 2), new Byte(0x77));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0x77)));
    }

    [Test]
    public void WriteOnlyOnTick_Works()
    {
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0xAA));
        _cpu.EVal(MakeInstruction(0, 6, 2), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0x00)));

        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0xAA));
        _cpu.EVal(MakeInstruction(0, 6, 2), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0xAA)));
    }

    [Test]
    public void Source7_Unused_ReturnsZero()
    {
        _cpu.EVal(MakeInstruction(6, 4, 2), new Byte(0xDD));
        _cpu.EVal(MakeInstruction(7, 6, 2), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0x00)));
    }

    [Test]
    public void Destination7_Unused_DoesNothing()
    {
        _cpu.EVal(MakeInstruction(6, 5, 2), new Byte(0xEE));
        _cpu.EVal(MakeInstruction(5, 7, 2), new Byte(0x00));
        _cpu.EVal(MakeInstruction(5, 6, 2), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0xEE)));
    }

    [Test]
    public void MultipleMoves_Work()
    {
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x01));
        _cpu.EVal(MakeInstruction(0, 1, 2), new Byte(0x00));
        _cpu.EVal(MakeInstruction(1, 2, 2), new Byte(0x00));
        _cpu.EVal(MakeInstruction(2, 6, 2), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0x01)));
    }

    // ==========================================
    // ALU PHASE TESTS
    // ==========================================

    [Test]
    public void ALU_AddsReg1AndReg2_StoresInReg3()
    {
        _cpu.EVal([1,0 ,0,0,1, 1,1,0], 0x0A); // Load 0x0A into Reg1
        _cpu.EVal([1,0 ,0,1,0 ,1,1,0], 0x05); // Load 0x05 into Reg2
        _cpu.EVal([0,1 ,0,0,0 ,1,0,0], 0x00); // ALU operation
        _cpu.EVal([1,0 ,1,1,0 ,0,1,1], 0x00); // Move result to output

        Byte output = (Byte)_cpu;
        Byte expected = new Byte(0x0F);

        Assert.That(output, Is.EqualTo(expected));
    }

    [Test]
    public void ALU_SubReg1MinusReg2_StoresInReg3()
    {
        _cpu.EVal(MakeInstruction(6, 1, 2), new Byte(0x0A));
        _cpu.EVal(MakeInstruction(6, 2, 2), new Byte(0x03));
        _cpu.EVal(MakeInstruction(5, 0, 1), new Byte(0x00));
        _cpu.EVal(MakeInstruction(3, 6, 2), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0x07)));
    }

    [Test]
    public void ALU_AndReg1AndReg2_StoresInReg3()
    {
        _cpu.EVal(MakeInstruction(6, 1, 2), new Byte(0x0F));
        _cpu.EVal(MakeInstruction(6, 2, 2), new Byte(0x33));
        _cpu.EVal(MakeInstruction(2, 0, 1), new Byte(0x00));
        _cpu.EVal(MakeInstruction(3, 6, 2), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0x03)));
    }

    [Test]
    public void ALU_DoesNotAffectOtherRegisters()
    {
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x55));
        _cpu.EVal(MakeInstruction(4, 0, 1), new Byte(0x00));
        _cpu.EVal(MakeInstruction(0, 6, 2), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0x55)));
    }

    [Test]
    public void ALU_DoesNotOutput_WhenInALU()
    {
        _cpu.EVal(MakeInstruction(6, 1, 2), new Byte(0x05));
        _cpu.EVal(MakeInstruction(6, 2, 2), new Byte(0x03));
        _cpu.EVal(MakeInstruction(4, 6, 1), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0x00)));
    }

    // ==========================================
    // IMMEDIATE PHASE TESTS
    // ==========================================

    [Test]
    public void Immediate_StoresLower6BitsIntoReg0()
    {
        _cpu.EVal(new Byte(0x2A), new Byte(0x00));
        _cpu.EVal(MakeInstruction(0, 6, 2), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0x2A)));
    }

    [Test]
    public void Immediate_IgnoresUpperBits()
    {
        _cpu.EVal(new Byte(0x3F), new Byte(0x00));
        _cpu.EVal(MakeInstruction(0, 6, 2), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0x3F)));
    }

    [Test]
    public void Immediate_DoesNotAffectOtherRegisters()
    {
        _cpu.EVal(MakeInstruction(6, 1, 2), new Byte(0xAA));
        _cpu.EVal(new Byte(0x12), new Byte(0x00));
        _cpu.EVal(MakeInstruction(1, 6, 2), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0xAA)));
    }

    // ==========================================
    // CONDITION PHASE TESTS
    // ==========================================

    [Test]
    public void Condition_EqualsZero_True()
    {
        // REG3 = 0, condition 010 (== 0) -> should jump
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0x00));
        // Load REG0 with 0x42 via INPUT (Move mode)
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42));
        _cpu.EVal(MakeInstruction(2, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_EqualsZero_False()
    {
        // REG3 = 5, condition 010 (== 0) -> should NOT jump
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0x05));
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42)); // REG0 = 0x42
        int before = _cpu.ProgramCounter;
        _cpu.EVal(MakeInstruction(2, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_Never_DoesNotJump()
    {
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0x00));
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal(MakeInstruction(0, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0x00)));
    }

    [Test]
    public void Condition_Always_Jumps()
    {
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42)); // REG0 = 0x42
        _cpu.EVal(MakeInstruction(1, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0x00)));
    }

    [Test]
    public void Condition_NotEqualsZero_True()
    {
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0x05));
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42));
        _cpu.EVal(MakeInstruction(3, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_NotEqualsZero_False()
    {
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0x00));
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal(MakeInstruction(3, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_LessThanZero_True()
    {
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0xFB)); // -5
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42));
        _cpu.EVal(MakeInstruction(4, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_LessThanZero_False()
    {
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0x05));
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal(MakeInstruction(4, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_GreaterThanZero_True()
    {
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0x05));
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42));
        _cpu.EVal(MakeInstruction(7, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_GreaterThanZero_False()
    {
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0xFB)); // -5
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal(MakeInstruction(7, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_GeZero_True()
    {
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0x00));
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42));
        _cpu.EVal(MakeInstruction(5, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));

        _cpu.Reset();
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0x05));
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42));
        _cpu.EVal(MakeInstruction(5, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_GeZero_False()
    {
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0xFB)); // -5
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal(MakeInstruction(5, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_LeZero_True()
    {
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0x00));
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42));
        _cpu.EVal(MakeInstruction(6, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));

        _cpu.Reset();
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0xFB)); // -5
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42));
        _cpu.EVal(MakeInstruction(6, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_LeZero_False()
    {
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0x05));
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal(MakeInstruction(6, 0, 3), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_DoesNotAffectRegisters()
    {
        _cpu.EVal(MakeInstruction(6, 3, 2), new Byte(0x05));
        _cpu.EVal(MakeInstruction(6, 0, 2), new Byte(0x42));
        _cpu.EVal(MakeInstruction(7, 0, 3), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0x00)));
        _cpu.EVal(MakeInstruction(0, 6, 2), new Byte(0x00));
        Assert.That((Byte)_cpu, Is.EqualTo(new Byte(0x42)));
    }
}