namespace Turing.Core.Executeables.Overture;

public partial class OvertureRunner
{
    public enum Mode
    {
        IMM = 0b00_000000,
        ALU = 0b01_000000,
        MOVE = 0b10_000000,
        CND = 0b11_000000,
    }
}
