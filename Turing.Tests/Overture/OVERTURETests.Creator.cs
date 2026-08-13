namespace Turing.Tests.Overture;

internal partial class OVERTURETests
{
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
        public static Byte Cnd(CndOp value) => Make((int)Mode.CND | (int)value);
    }
}
