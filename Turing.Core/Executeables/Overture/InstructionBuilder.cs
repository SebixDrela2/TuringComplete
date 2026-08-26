namespace Turing.Core.Executeables.Overture;

public class InstructionBuilder
{
    private readonly List<Byte> _instructions = [];
    private int Offset => _instructions.Count;

    private InstructionBuilder Make(int bits)
    {
        _instructions.Add((Byte)(byte)bits);

        return this;
    }
    public InstructionBuilder Imm(int value) => Make(value & 0b00111111);
    public InstructionBuilder Imm(Label label)
    {
        var offset = Offset;

        label.Complete += (address) => _instructions[offset] = address;

        return this;
    }
    public InstructionBuilder Alu(AluOp value) => Make((int)Mode.ALU | (int)value);
    public InstructionBuilder Move(Reg output, Reg input) => Make((int)Mode.MOVE | ((int)output << 3) | (int)input);
    public InstructionBuilder Off() => Make((int)Mode.MOVE | (0b111 << 3) | 0b111);
    public InstructionBuilder Cnd(CndOp value) => Make((int)Mode.CND | (int)value);
    public InstructionBuilder Label(Label label)
    {
        label.Complete?.Invoke(Offset);

        return this;
    }
    public InstructionBuilder Scope(out Label label)
    {
        label = new Label();

        return this;
    }
    public Byte[] Build() => [.. _instructions];
}
