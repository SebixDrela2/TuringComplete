namespace Turing.Tests.Overture;

internal partial class OVERTURETests
{
    [Test]
    public void Move_RegisterToRegister_Works()
    {
        var cpu = RunOverture([
            Instruction.Move(Reg.Reg0, Reg.Input),
            Instruction.Move(Reg.Reg1, Reg.Reg0),
            Instruction.Move(Reg.Output, Reg.Reg1)
        ], new Byte(0xAA));

        Assert.That(cpu.Output, Is.EqualTo(new Byte(0xAA)));
        Assert.That(cpu.ProgramCounter, Is.EqualTo(3));
    }

    [Test]
    public void Move_InputToRegister_Works()
    {
        var cpu = RunOverture([
            Instruction.Move(Reg.Reg2, Reg.Input),
            Instruction.Move(Reg.Output, Reg.Reg2)
        ], new Byte(0x55));

        Assert.That(cpu.Output, Is.EqualTo(new Byte(0x55)));
    }

    [Test]
    public void Move_RegisterToOutput_Works()
    {
        var cpu = RunOverture([
            Instruction.Move(Reg.Reg3, Reg.Input),
            Instruction.Move(Reg.Output, Reg.Reg3)
        ], new Byte(0xCC));

        Assert.That(cpu.Output, Is.EqualTo(new Byte(0xCC)));
    }

    [Test]
    public void Move_InputToOutput_Works()
    {
        var cpu = RunOverture([
            Instruction.Move(Reg.Output, Reg.Input)
        ], new Byte(0x77));

        Assert.That(cpu.Output, Is.EqualTo(new Byte(0x77)));
    }

    [Test]
    public void WriteOnlyOnTick_Works()
    {
        var cpu = RunOverture([
            Instruction.Move(Reg.Reg0, Reg.Input),
            Instruction.Move(Reg.Output, Reg.Reg0)
        ], new Byte(0xAA));

        Assert.That(cpu.Output, Is.EqualTo(new Byte(0xAA)));
    }

    [Test]
    public void MultipleMoves_Work()
    {
        var cpu = RunOverture([
            Instruction.Move(Reg.Reg0, Reg.Input),
            Instruction.Move(Reg.Reg1, Reg.Reg0),
            Instruction.Move(Reg.Reg2, Reg.Reg1),
            Instruction.Move(Reg.Output, Reg.Reg2)
        ], new Byte(0x01));

        Assert.That(cpu.Output, Is.EqualTo(new Byte(0x01)));
    }

    // ==========================================
    // ALU PHASE TESTS
    // ==========================================

    [Test]
    public void ALU_AddsReg1AndReg2_StoresInReg3()
    {
        var cpu = RunOverture([
            Instruction.Move(Reg.Reg1, Reg.Input),
            Instruction.Move(Reg.Reg2, Reg.Input),
            Instruction.Alu(AluOp.ADD),
            Instruction.Move(Reg.Output, Reg.Reg3)
        ], new Byte(0x0A), new Byte(0x05));

        Assert.That(cpu.Output, Is.EqualTo(new Byte(0x0F)));
    }

    [Test]
    public void ALU_SubReg1MinusReg2_StoresInReg3()
    {
        var cpu = RunOverture([
            Instruction.Move(Reg.Reg1, Reg.Input),
            Instruction.Move(Reg.Reg2, Reg.Input),
            Instruction.Alu(AluOp.SUB),
            Instruction.Move(Reg.Output, Reg.Reg3)
        ], new Byte(0x0A), new Byte(0x03));

        Assert.That(cpu.Output, Is.EqualTo(new Byte(0x07)));
    }

    [Test]
    public void ALU_AndReg1AndReg2_StoresInReg3()
    {
        var cpu = RunOverture([
            Instruction.Move(Reg.Reg1, Reg.Input),
            Instruction.Move(Reg.Reg2, Reg.Input),
            Instruction.Alu(AluOp.AND),
            Instruction.Move(Reg.Output, Reg.Reg3)
        ], new Byte(0x0F), new Byte(0x33));

        Assert.That(cpu.Output, Is.EqualTo(new Byte(0x03)));
    }

    [Test]
    public void ALU_DoesNotAffectOtherRegisters()
    {
        var cpu = RunOverture([
            Instruction.Move(Reg.Reg0, Reg.Input),
            Instruction.Alu(AluOp.ADD),
            Instruction.Move(Reg.Output, Reg.Reg0)
        ], new Byte(0x55));

        Assert.That(cpu.Output, Is.EqualTo(new Byte(0x55)));
    }

    [Test]
    public void ALU_DoesNotOutput_WhenInALU()
    {
        var cpu = RunOverture([
            Instruction.Move(Reg.Reg1, Reg.Input),
            Instruction.Move(Reg.Reg2, Reg.Input),
            Instruction.Alu(AluOp.ADD)
        ], new Byte(0x05), new Byte(0x03));

        Assert.That(cpu.Output, Is.EqualTo(new Byte(0x00)));
    }

    // ==========================================
    // IMMEDIATE PHASE TESTS
    // ==========================================

    [Test]
    public void Immediate_StoresLower6BitsIntoReg0()
    {
        var cpu = RunOverture([
            Instruction.Imm(0x2A),
            Instruction.Move(Reg.Output, Reg.Reg0)
        ]);

        Assert.That(cpu.Output, Is.EqualTo(new Byte(0x2A)));
    }

    [Test]
    public void Immediate_IgnoresUpperBits()
    {
        var cpu = RunOverture([
            Instruction.Imm(0x3F),
            Instruction.Move(Reg.Output, Reg.Reg0)
        ]);

        Assert.That(cpu.Output, Is.EqualTo(new Byte(0x3F)));
    }

    [Test]
    public void Immediate_DoesNotAffectOtherRegisters()
    {
        var cpu = RunOverture([
            Instruction.Move(Reg.Reg1, Reg.Input),
            Instruction.Imm(0x12),
            Instruction.Move(Reg.Output, Reg.Reg1)
        ], new Byte(0xAA));

        Assert.That(cpu.Output, Is.EqualTo(new Byte(0xAA)));
    }

    // ==========================================
    // CONDITION PHASE TESTS
    // ==========================================

    [Test]
    public void Condition_EqualsZero_True()
    {
        var instructions = new InstructionBuilder()
            .Move(Reg.Reg3, Reg.Input)
            .Move(Reg.Reg0, Reg.Input)
            .Cnd(CndOp.Equal)
            .Build();

        var cpu = RunOverture(instructions, new Byte(0x0), new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_EqualsZero_False()
    {
        var instructions = new InstructionBuilder()
            .Move(Reg.Reg3, Reg.Input)
            .Move(Reg.Reg0, Reg.Input)
            .Cnd(CndOp.Equal)
            .Build();

        var cpu = RunOverture(instructions, new Byte(0x05), new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(instructions.Length));
    }

    [Test]
    public void Condition_Never_DoesNotJump()
    {
        var instructions = new InstructionBuilder()
            .Move(Reg.Reg3, Reg.Input)
            .Move(Reg.Reg0, Reg.Input)
            .Cnd(CndOp.Never)
            .Build();
        var cpu = RunOverture(instructions, new Byte(0x00), new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(instructions.Length));
    }

    [Test]
    public void Condition_Always_Jumps()
    {
        var instructions = new InstructionBuilder()
            .Move(Reg.Reg0, Reg.Input)
            .Cnd(CndOp.Always)
            .Build();

        var cpu = RunOverture(instructions, new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_NotEqualsZero_True()
    {
        var instructions = new InstructionBuilder()
            .Move(Reg.Reg3, Reg.Input)
            .Move(Reg.Reg0, Reg.Input)
            .Cnd(CndOp.NotEqual)
            .Build();

        var cpu = RunOverture(instructions, new Byte(0x05), new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_NotEqualsZero_False()
    {
        var instructions = new InstructionBuilder()
           .Move(Reg.Reg3, Reg.Input)
           .Move(Reg.Reg0, Reg.Input)
           .Cnd(CndOp.NotEqual)
           .Build();

        var cpu = RunOverture(instructions, new Byte(0x00), new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(instructions.Length));
    }

    [Test]
    public void Condition_LessThanZero_True()
    {
        var instructions = new InstructionBuilder()
           .Move(Reg.Reg3, Reg.Input)
           .Move(Reg.Reg0, Reg.Input)
           .Cnd(CndOp.Less)
           .Build();

        var cpu = RunOverture(instructions, new Byte(0xFB), new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_LessThanZero_False()
    {
        var instructions = new InstructionBuilder()
           .Move(Reg.Reg3, Reg.Input)
           .Move(Reg.Reg0, Reg.Input)
           .Cnd(CndOp.Less)
           .Build();

        var cpu = RunOverture(instructions, new Byte(0x05), new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(instructions.Length));
    }

    [Test]
    public void Condition_GreaterThanZero_True()
    {
        var instructions = new InstructionBuilder()
           .Move(Reg.Reg3, Reg.Input)
           .Move(Reg.Reg0, Reg.Input)
           .Cnd(CndOp.Greater)
           .Build();

        var cpu = RunOverture(instructions, new Byte(0x05), new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_GreaterThanZero_False()
    {
        var instructions = new InstructionBuilder()
           .Move(Reg.Reg3, Reg.Input)
           .Move(Reg.Reg0, Reg.Input)
           .Cnd(CndOp.Greater)
           .Build();

        var cpu = RunOverture(instructions, new Byte(0xFB), new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(instructions.Length));
    }

    [Test]
    public void Condition_GeZero_True()
    {
        var instructions = new InstructionBuilder()
           .Move(Reg.Reg3, Reg.Input)
           .Move(Reg.Reg0, Reg.Input)
           .Cnd(CndOp.GreaterOrEqual)
           .Build();

        var cpu = RunOverture(instructions, new Byte(0x00), new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(0x42));

        cpu = RunOverture(instructions, new Byte(0x05), new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_GeZero_False()
    {
        var instructions = new InstructionBuilder()
           .Move(Reg.Reg3, Reg.Input)
           .Move(Reg.Reg0, Reg.Input)
           .Cnd(CndOp.GreaterOrEqual)
           .Build();

        var cpu = RunOverture(instructions, new Byte(0xFB), new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(instructions.Length));
    }

    [Test]
    public void Condition_LeZero_True()
    {
        var instructions = new InstructionBuilder()
          .Move(Reg.Reg3, Reg.Input)
          .Move(Reg.Reg0, Reg.Input)
          .Cnd(CndOp.LessOrEqual)
          .Build();

        var cpu = RunOverture(instructions, new Byte(0x00), new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(0x42));

        cpu = RunOverture(instructions, new Byte(0xFB), new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(0x42));
    }

    [Test]
    public void Condition_LeZero_False()
    {
        var instructions = new InstructionBuilder()
          .Move(Reg.Reg3, Reg.Input)
          .Move(Reg.Reg0, Reg.Input)
          .Cnd(CndOp.LessOrEqual)
          .Build();

        var cpu = RunOverture(instructions, new Byte(0x05), new Byte(0x42));

        Assert.That(cpu.ProgramCounter, Is.EqualTo(instructions.Length));
    }

    [Test]
    public void Condition_DoesNotAffectRegisters()
    {
        var instructions = new InstructionBuilder()
          .Move(Reg.Reg3, Reg.Input)
          .Move(Reg.Reg0, Reg.Input)
          .Cnd(CndOp.Greater)
          .Move(Reg.Output, Reg.Reg0)
          .Build();

        var cpu = RunOverture(instructions, new Byte(0x00), new Byte(0x42));

        Assert.That(cpu.Output, Is.EqualTo(new Byte(0x42)));
    }

    [Test]
    [TestCase(0x00, 0x00)]  // Radius 0 -> Circumference 0
    [TestCase(0x01, 0x06)]  // Radius 1 -> Circumference 6 (6 * 1)
    [TestCase(0x02, 0x0C)]  // Radius 2 -> Circumference 12 (6 * 2)
    [TestCase(0x03, 0x12)]  // Radius 3 -> Circumference 18 (6 * 3)
    [TestCase(0x04, 0x18)]  // Radius 4 -> Circumference 24 (6 * 4)
    [TestCase(0x05, 0x1E)]  // Radius 5 -> Circumference 30 (6 * 5)
    [TestCase(0x06, 0x24)]  // Radius 6 -> Circumference 36 (6 * 6)
    [TestCase(0x07, 0x2A)]  // Radius 7 -> Circumference 42 (6 * 7)
    [TestCase(0x08, 0x30)]  // Radius 8 -> Circumference 48 (6 * 8)
    [TestCase(0x09, 0x36)]  // Radius 9 -> Circumference 54 (6 * 9)
    [TestCase(0x0A, 0x3C)]  // Radius 10 -> Circumference 60 (6 * 10)
    public void CircumferenceCalculator_WithPiEquals3_Works(byte radius, int expectedCircumference)
    {
        var instructions = new InstructionBuilder()
            .Move(Reg.Reg1, Reg.Input)
            .Imm(1)
            .Move(Reg.Reg2, Reg.Reg0)
            .Move(Reg.Reg3, Reg.Reg1)
            .Imm(23)
            .Cnd(CndOp.Equal)
            .Imm(1)
            .Move(Reg.Reg2, Reg.Reg0)
            .Alu(AluOp.SUB)
            .Move(Reg.Reg4, Reg.Reg3)
            .Move(Reg.Reg2, Reg.Reg0)
            .Imm(6)
            .Move(Reg.Reg2, Reg.Reg0)
            .Move(Reg.Reg1, Reg.Reg5)
            .Alu(AluOp.ADD)
            .Move(Reg.Reg5, Reg.Reg3)
            .Imm(1)
            .Move(Reg.Reg2, Reg.Reg0)
            .Move(Reg.Reg3, Reg.Reg4)
            .Move(Reg.Reg1, Reg.Reg3)
            .Imm(6)
            .Cnd(CndOp.NotEqual)
            .Move(Reg.Output, Reg.Reg5)
            .Build();

        var cpu = RunOverture(instructions, new Byte(radius));

        Assert.That(cpu.Output, Is.EqualTo(new Byte(expectedCircumference)));
    }
}