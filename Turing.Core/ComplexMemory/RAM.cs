using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Turing.Core.Components;
using Turing.Core.Electricity;

namespace Turing.Core.ComplexMemory;

[Component(Primitive = true)]
public class RAM
{
    private readonly Byte[] _state;

    public RAM(int size)
    {
        _state = new Byte[size * sizeof(int)];
    }

    public RAM(params Byte[] instructions)
    {
        _state = new Byte[1 << Short.BitWidth];

        instructions.CopyTo(_state);
        _state[instructions.Length + 3] = 0x80;
    }

    public Int Load(Int address)
    {
        var byte1 = (byte)_state[address];
        var byte2 = (byte)_state[address + 1];
        var byte3 = (byte)_state[address + 2];
        var byte4 = (byte)_state[address + 3];

        Int result = MemoryMarshal.Read<int>([byte1, byte2, byte3, byte4]);

        return result;
    }

    public void Write(Int address, Int value, Bit enabled)
    {
        if (enabled)
        {
            int intValue = value;
            var bytes = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref intValue, 1));

            _state[address] = bytes[0];
            _state[address + 1] = bytes[1];
            _state[address + 2] = bytes[2];
            _state[address + 3] = bytes[3];
        }
    }
}
