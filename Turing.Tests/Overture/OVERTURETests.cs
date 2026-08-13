using Turing.Core.Overture;

namespace Turing.Tests.Overture;

internal partial class OVERTURETests
{
    private OVERTURE _cpu;

    [SetUp]
    public void SetUp()
    {
        _cpu = new OVERTURE();
    }

    [Test]
    public void Move_RegisterToRegister_Works()
    {
        // Load 0xAA into Reg0: MOVE Input -> Reg0
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0xAA));
        // Move Reg0 -> Reg1
        _cpu.EVal(Instruction.Move(Reg.Reg1, Reg.Reg0), new Byte(0x00));
        // Move Reg1 -> Output
        _cpu.EVal(Instruction.Move(Reg.Output, Reg.Reg1), new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0xAA)));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(3));
    }

    [Test]
    public void Move_InputToRegister_Works()
    {
        // Load input to Reg2
        _cpu.EVal(Instruction.Move(Reg.Reg2, Reg.Input), new Byte(0x55));
        // Move Reg2 -> Output
        _cpu.EVal(Instruction.Move(Reg.Output, Reg.Reg2), new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x55)));
    }

    [Test]
    public void Move_RegisterToOutput_Works()
    {
        // Load input to Reg3
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0xCC));
        // Move Reg3 -> Output
        _cpu.EVal(Instruction.Move(Reg.Output, Reg.Reg3), new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0xCC)));
    }

    [Test]
    public void Move_InputToOutput_Works()
    {
        // Move input directly to Output
        _cpu.EVal(Instruction.Move(Reg.Output, Reg.Input), new Byte(0x77));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x77)));
    }

    [Test]
    public void WriteOnlyOnTick_Works()
    {
        // Load Reg0
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0xAA));
        // Move Reg0 -> Output
        _cpu.EVal(Instruction.Move(Reg.Output, Reg.Reg0), new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0xAA)));
    }

    [Test]
    public void MultipleMoves_Work()
    {
        // Load 0x01 into Reg0
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x01));
        // Move Reg0 -> Reg1
        _cpu.EVal(Instruction.Move(Reg.Reg1, Reg.Reg0), new Byte(0x00));
        // Move Reg1 -> Reg2
        _cpu.EVal(Instruction.Move(Reg.Reg2, Reg.Reg1), new Byte(0x00));
        // Move Reg2 -> Output
        _cpu.EVal(Instruction.Move(Reg.Output, Reg.Reg2), new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x01)));
    }

    // ==========================================
    // ALU PHASE TESTS (ALU mode: bit6=1, bit7=0 -> ..., 1, 0)
    // ==========================================

    [Test]
    public void ALU_AddsReg1AndReg2_StoresInReg3()
    {
        // Load 0x0A into Reg1
        _cpu.EVal(Instruction.Move(Reg.Reg1, Reg.Input), new Byte(0x0A));
        // Load 0x05 into Reg2
        _cpu.EVal(Instruction.Move(Reg.Reg2, Reg.Input), new Byte(0x05));
        // ALU ADD (stores result in Reg3 by default)
        _cpu.EVal(Instruction.Alu(AluOp.ADD), new Byte(0x00));
        // Move Reg3 -> Output
        _cpu.EVal(Instruction.Move(Reg.Output, Reg.Reg3), new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x0F)));
    }

    [Test]
    public void ALU_SubReg1MinusReg2_StoresInReg3()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg1, Reg.Input), new Byte(0x0A));
        _cpu.EVal(Instruction.Move(Reg.Reg2, Reg.Input), new Byte(0x03));
        // ALU SUB
        _cpu.EVal(Instruction.Alu(AluOp.SUB), new Byte(0x00));
        _cpu.EVal(Instruction.Move(Reg.Output, Reg.Reg3), new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x07)));
    }

    [Test]
    public void ALU_AndReg1AndReg2_StoresInReg3()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg1, Reg.Input), new Byte(0x0F));
        _cpu.EVal(Instruction.Move(Reg.Reg2, Reg.Input), new Byte(0x33));
        // ALU AND
        _cpu.EVal(Instruction.Alu(AluOp.AND), new Byte(0x00));
        _cpu.EVal(Instruction.Move(Reg.Output, Reg.Reg3), new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x03)));
    }

    [Test]
    public void ALU_DoesNotAffectOtherRegisters()
    {
        // Load Reg0 with 0x55
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x55));
        // ALU ADD
        _cpu.EVal(Instruction.Alu(AluOp.ADD), new Byte(0x00));
        // Move Reg0 -> Output
        _cpu.EVal(Instruction.Move(Reg.Output, Reg.Reg0), new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x55)));
    }

    [Test]
    public void ALU_DoesNotOutput_WhenInALU()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg1, Reg.Input), new Byte(0x05));
        _cpu.EVal(Instruction.Move(Reg.Reg2, Reg.Input), new Byte(0x03));
        _cpu.EVal(Instruction.Alu(AluOp.ADD), new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x00)));
    }

    // ==========================================
    // IMMEDIATE PHASE TESTS (IMM mode: bit6=0, bit7=0 -> ..., 0, 0)
    // ==========================================

    [Test]
    public void Immediate_StoresLower6BitsIntoReg0()
    {
        // IMM mode with value 0x2A
        _cpu.EVal(Instruction.Imm(0x2A), new Byte(0x00));
        // Move Reg0 -> Output
        _cpu.EVal(Instruction.Move(Reg.Output, Reg.Reg0), new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x2A)));
    }

    [Test]
    public void Immediate_IgnoresUpperBits()
    {
        _cpu.EVal(Instruction.Imm(0x3F), new Byte(0x00));
        _cpu.EVal(Instruction.Move(Reg.Output, Reg.Reg0), new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x3F)));
    }

    [Test]
    public void Immediate_DoesNotAffectOtherRegisters()
    {
        // Load Reg1 with 0xAA
        _cpu.EVal(Instruction.Move(Reg.Reg1, Reg.Input), new Byte(0xAA));
        // Immediate 0x12 to Reg0
        _cpu.EVal(Instruction.Imm(0x12), new Byte(0x00));
        // Move Reg1 -> Output
        _cpu.EVal(Instruction.Move(Reg.Output, Reg.Reg1), new Byte(0x00));

        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0xAA)));
    }

    // ==========================================
    // CONDITION PHASE TESTS (COND mode: bit6=1, bit7=1 -> ..., 1, 1)
    // ==========================================

    [Test]
    public void Condition_EqualsZero_True()
    {
        // Reg3 = 0
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0x00));
        // Reg0 = 0x42
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        // COND == 0
        _cpu.EVal(Instruction.Cnd(CndOp.Equal), new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_EqualsZero_False()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0x05));
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal(Instruction.Cnd(CndOp.Equal), new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_Never_DoesNotJump()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0x00));
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal(Instruction.Cnd(CndOp.Never), new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_Always_Jumps()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        _cpu.EVal(Instruction.Cnd(CndOp.Always), new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_NotEqualsZero_True()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0x05));
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        _cpu.EVal(Instruction.Cnd(CndOp.NotEqual), new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_NotEqualsZero_False()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0x00));
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal(Instruction.Cnd(CndOp.NotEqual), new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_LessThanZero_True()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0xFB));
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        _cpu.EVal(Instruction.Cnd(CndOp.Less), new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_LessThanZero_False()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0x05));
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal(Instruction.Cnd(CndOp.Less), new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_GreaterThanZero_True()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0x05));
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        _cpu.EVal(Instruction.Cnd(CndOp.Greater), new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_GreaterThanZero_False()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0xFB));
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal(Instruction.Cnd(CndOp.Greater), new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_GeZero_True()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0x00));
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        _cpu.EVal(Instruction.Cnd(CndOp.GreaterOrEqual), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));

        _cpu.Reset();
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0x05));
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        _cpu.EVal(Instruction.Cnd(CndOp.GreaterOrEqual), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_GeZero_False()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0xFB));
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal(Instruction.Cnd(CndOp.GreaterOrEqual), new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_LeZero_True()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0x00));
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        _cpu.EVal(Instruction.Cnd(CndOp.LessOrEqual), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));

        _cpu.Reset();
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0xFB));
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        _cpu.EVal(Instruction.Cnd(CndOp.LessOrEqual), new Byte(0x00));
        Assert.That(_cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_LeZero_False()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0x05));
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        int before = _cpu.ProgramCounter;
        _cpu.EVal(Instruction.Cnd(CndOp.LessOrEqual), new Byte(0x00));

        Assert.That(_cpu.ProgramCounter, Is.EqualTo(before + 1));
    }

    [Test]
    public void Condition_DoesNotAffectRegisters()
    {
        _cpu.EVal(Instruction.Move(Reg.Reg3, Reg.Input), new Byte(0x05));
        _cpu.EVal(Instruction.Move(Reg.Reg0, Reg.Input), new Byte(0x42));
        _cpu.EVal(Instruction.Cnd(CndOp.Greater), new Byte(0x00)); // >0, jumps
        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x00)));

        // Move Reg0 -> Output
        _cpu.EVal(Instruction.Move(Reg.Output, Reg.Reg0), new Byte(0x00));
        Assert.That(_cpu.GetOutput(), Is.EqualTo(new Byte(0x42)));
    }
}