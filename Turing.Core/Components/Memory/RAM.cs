namespace Turing.Core.Components.Memory;

/// <summary>
/// <br>RAM is usually a component made by given set of registers coupled together.</br>
/// <br>However for simplicity, as well as operational advantage this component is implemented by "Primitive".</br>
/// <br>Given it uses actual C# methods to simulate real deal.</br>
/// </summary>
[Component(Primitive = true)]
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
