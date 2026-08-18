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
        _regRam = new RAM(Clock, Short.BitWidth);
        _ram = new RAM(Clock, instructions);

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
        Int mux = new MUX<Int>(loadB, immVal, isImm);

        _ram.Write(mux, loadA.Into<Byte>(), new AND<Bit>(new EQ<Byte>(opCode, 4), ram));
        _ram.Write(mux, loadA.Into<Short>(), new AND<Bit>(new EQ<Byte>(opCode, 5), ram));
        _ram.Write(mux, loadA, new AND<Bit>(new EQ<Byte>(opCode, 6), ram));

        Int ramLoad_8 = new SW<Int>(new AND<Bit>(new EQ<Byte>(opCode, 0), ram), _ram.Load(mux)).Into<Byte>();
        Int ramLoad_16 = new SW<Int>(new AND<Bit>(new EQ<Byte>(opCode, 1), ram), _ram.Load(mux)).Into<Short>();
        Int ramLoad_32 = new SW<Int>(new AND<Bit>(new EQ<Byte>(opCode, 2), ram), _ram.Load(mux));
        Int ramLoad = new OR<Int>(new OR<Int>(ramLoad_8, ramLoad_16), ramLoad_32);

        InputPin = new AND<Bit>(io, new EQ<Byte>(opCode, 1));
        Int inputData = new SW<Int>(InputPin, Input);
        Int aluResult = new SW<Int>(alu, new ALU(opCode, loadA, mux, alu));
        Int dstFlow = new OR<Int>(ramLoad, new OR<Int>(inputData, aluResult));

        _regRam.Write(dstAddress, dstFlow, destNotZero);
        OutputPin = new AND<Bit>(io, new EQ<Byte>(opCode, 2));
        _output = new SW<Int>(OutputPin, mux);
        OffPin = instruction.LastBit();

        Bit flags = new COND(loadA.Into<Byte>(), opCode);
        Bit isJump = new AND<Bit>(flags, jump);

        _pc.EVal(isJump, mux);
    }
}
