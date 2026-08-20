using Turing.Core.Electricity;

namespace Turing.Core.Gates;

/// <summary>
/// <br>NAND gate, is the one that rules them all since it has special capability.</br>
/// <br>Every piece of electronic hardware can be made just out of NAND gates as every other gate can be made out of a NAND.</br>
/// <br>NAND only outputs OFF when both pins are ON, otherwise it outputs ON.</br>
/// </summary>
public class NAND<T>(T inputA, T inputB) : TurComponentValue<T>, IGate<T, T> where T : struct, IValue<T>
{
    protected override T ImplicitOperator() => new NOT<T>(new AND<T>(inputA, inputB));
}