namespace Turing.Core.Executeables.Overture;

public partial class OvertureRunner
{
    public enum Reg
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
}
