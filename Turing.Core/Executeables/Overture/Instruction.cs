namespace Turing.Core.Executeables.Overture;

public partial class OvertureRunner
{
    public static class Instruction
    {
        private static Byte Make(int bits) => (Byte)(byte)bits;
        public static Byte Imm(int value) => Make(value & 0b00111111);
        public static Byte Alu(AluOp value) => Make((int)Mode.ALU | (int)value);
        public static Byte Move(Reg output, Reg input) => Make((int)Mode.MOVE | ((int)output << 3) | (int)input);
        public static Byte Off() => Make((int)Mode.MOVE | (0b111 << 3) | 0b111);
        public static Byte Cnd(CndOp value) => Make((int)Mode.CND | (int)value);
    }
}
