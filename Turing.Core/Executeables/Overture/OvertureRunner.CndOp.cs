namespace Turing.Core.Executeables.Overture;

public partial class OvertureRunner
{
    public enum CndOp
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
}
