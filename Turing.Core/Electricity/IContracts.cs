namespace Turing.Core.Electricity;

public interface IValue<T> where T : struct, IValue<T>
{
    T Value { get; }
    static abstract T Zero { get; }
    static abstract int BitWidth { get; }
    T FromValue(bool value);
    T FromBits(bool[] bits);
    Bit GetBit(int index);
    T SetBit(int index, bool value);

    abstract static implicit operator T(bool value);
    abstract static implicit operator T(int value);
}

public interface IByteValue<T> : IValue<T> where T : struct, IByteValue<T>;

public interface IBitValue<T> : IValue<T> where T : struct, IBitValue<T>;

public interface IGate<TInput, TOutput> 
    where TInput : struct, IValue<TInput> 
    where TOutput : struct, IValue<TOutput>;

public interface IUnaryGate<TInput, TOutput>
     where TInput : struct, IValue<TInput>
    where TOutput : struct, IValue<TOutput>
{
    TOutput Evaluate(TInput a);
}

public interface IStateGate<T> where T : struct, IValue<T>
{
    T State { get; }
    void Reset();
}
