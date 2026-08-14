using Turing.Core.Overture;

namespace Turing.Tests.Overture;

internal partial class OVERTURETests
{
    private OVERTURE RunOverture(Byte[] instructions, params IEnumerable<Byte> inputs)
    {
        var cpu = new OVERTURE(instructions);

        var en = inputs.GetEnumerator();
        cpu.Input = en.MoveNext() ? en.Current : default;

        while(true)
        {
            cpu.EVal();

            if (cpu.InputPin)
            {
                cpu.Input = en.MoveNext() ? en.Current : default;
            }

            if (cpu.OffPin || cpu.OutputPin)
            {
                return cpu;
            }
        }
    }

    enum Mode
    {
        IMM = 0b00_000000,
        ALU = 0b01_000000,
        MOVE = 0b10_000000,
        CND = 0b11_000000,
    }
    enum AluOp
    {
        NAND = 0b000,
        OR = 0b001,
        AND = 0b010,
        NOR = 0b011,
        ADD = 0b100,
        SUB = 0b101,
    }
    enum CndOp
    {
        Never = 0b000,
        Always = 0b001,
        Equal = 0b010,
        NotEqual = 0b11,
        Less = 0b100,
        GreaterOrEqual = 0b101,
        LessOrEqual = 0b110,
        Greater = 0b111,
    }
    enum Reg
    {
        Reg0 = 0b000,
        Reg1 = 0b001,
        Reg2 = 0b010,
        Reg3 = 0b011,
        Reg4 = 0b100,
        Reg5 = 0b101,
        Input = 0b110,
        Output = 0b110,
    }
    private static class Instruction
    {
        private static Byte Make(int bits) => (Byte)(byte)bits;
        public static Byte Imm(int value) => Make(value & 0b00111111);
        public static Byte Alu(AluOp value) => Make((int)Mode.ALU | (int)value);
        public static Byte Move(Reg output, Reg input) => Make((int)Mode.MOVE | ((int)output << 3) | (int)input);
        public static Byte Off() => Make((int)Mode.MOVE | (0b111 << 3) | 0b111);
        public static Byte Cnd(CndOp value) => Make((int)Mode.CND | (int)value);
    }

    private class InstructionBuilder
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
        public Byte[] Build() => [.._instructions];
    }

    internal class Label
    {
        public Action<Byte>? Complete;

    }
}
