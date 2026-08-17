using Turing.Core.ComplexComponents;
using Turing.Core.ComplexMemory;
using Turing.Core.Components.Logic;
using Turing.Core.Computers;
using Turing.Core.Electricity;
using Turing.Core.Gates;
using Turing.Core.Gates.Primitives;
using Turing.Core.Symphony;

namespace Turing.Core.Computer.Symphony;

public class SYMPHONY : Processor
{
    private readonly COUNTER<Int> _pc;
    private readonly RAM _ram;
    private readonly RAM _regRam;

    //private Int ZERO_REG => _regs[0];
    //private Int SP => _regs[14];
    //private Int FLAGS => _regs[15];

    private Int _output;

    public Bit InputPin { get; private set; }
    public Bit OutputPin { get; private set; }
    public Bit OffPin { get; private set; }

    public Int Input { get; set; }
    public Int Output => _output;

    public SYMPHONY(params Byte[] instructions)
    {
        _regRam = new RAM(Short.BitWidth);
        _ram = new RAM(instructions);

        _pc = new COUNTER<Int>(Clock);
        _output = new Int(0);
    }

    protected override void Step()
    {
        Int instruction = _ram.Load(_pc);

        var (mode, opCode, destination, A, B, isImm, immVal) = ((Byte, Byte, Byte, Byte, Byte, Bit, Short)) new INSTRUCTION_DECODER(instruction);

        var (io, alu, jump, ram) = ((Bit, Bit, Bit, Bit))new MODE_DECODER(mode);

        Bit destNotZero = new NOT<Bit>(new EQ<Byte>(destination, Byte.Zero));

        Byte addressA = new SINDEXER<Byte>(A, -2);
        Byte addressB = new SINDEXER<Byte>(B, -2);
        Byte dstAddress = new SINDEXER<Byte>(destination, -2);

        Int loadA = _regRam.Load(addressA);
        Int loadB = _regRam.Load(addressB);
        Int ramOpsAddress = new MUX<Int>(B, immVal, isImm);

        _ram.Write(ramOpsAddress, loadA.Into<Byte>(), new AND<Bit>(opCode.GetBit(4), ram));
        _ram.Write(ramOpsAddress, loadA.Into<Short>(), new AND<Bit>(opCode.GetBit(5), ram));
        _ram.Write(ramOpsAddress, loadA, new AND<Bit>(opCode.GetBit(6), ram));

        Int ramLoad_8 = new SW<Int>(new AND<Bit>(opCode.GetBit(0), ram), _ram.Load(ramOpsAddress)).Into<Byte>();
        Int ramLoad_16 = new SW<Int>(new AND<Bit>(opCode.GetBit(1), ram), _ram.Load(ramOpsAddress)).Into<Short>();
        Int ramLoad_32 = new SW<Int>(new AND<Bit>(opCode.GetBit(2), ram), _ram.Load(ramOpsAddress));
        Int ramLoad = new OR<Int>(new OR<Int>(ramLoad_8, ramLoad_16), ramLoad_32);

        Int inputData = new SW<Int>(new AND<Bit>(io, opCode.GetBit(1)), Input);
        Int aluInput = new MUX<Int>(loadB, immVal, isImm);
        Int aluResult = new SW<Int>(alu, new ALU(opCode, loadA, aluInput, alu));
        Int dstFlow = new OR<Int>(ramLoad, new OR<Int>(inputData, aluResult));

        _regRam.Write(dstAddress, dstFlow, destNotZero);
        _output = new SW<Int>(new AND<Bit>(io, opCode.GetBit(2)), loadB);

        Bit flags = new COND(loadA.Into<Byte>(), opCode);
        Bit isJump = new AND<Bit>(flags, jump);

        _pc.EVal(isJump, loadB);
    }
}
