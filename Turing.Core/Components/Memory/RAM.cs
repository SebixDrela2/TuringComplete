namespace Turing.Core.Components.Memory;
public class RAM
{
    private const byte Off = 0b10111111;
    private readonly Byte[] _state;

    public RAM(params Byte[] instructions)
    {
        _state = new Byte[1 << Byte.BitWidth];

        instructions.CopyTo(_state);
        _state.AsSpan(instructions.Length).Fill(Off);
    }

    public Byte EVal(Byte address) => _state[address];
}
