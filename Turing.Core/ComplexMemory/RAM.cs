namespace Turing.Core.ComplexMemory;

public class RAM
{
    public static byte Off => throw new NotImplementedException();
    private readonly Byte[] _state;

    public RAM(params Byte[] instructions)
    {
        _state = new Byte[1 << Short.BitWidth];

        instructions.CopyTo(_state);
        _state.AsSpan(instructions.Length).Fill(Off);
    }

    public Byte EVal(Byte address) => _state[address];
}
