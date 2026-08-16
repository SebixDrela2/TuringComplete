namespace Turing.Core.Electricity;

public interface IValue
{
    bool[] ToBits();
}

public interface IValue<T> : IValue where T : struct, IValue<T>
{
    T Value { get; }
    Bit GetBit(int index);
    void SetBit(int index, bool value);
    static abstract T Zero { get; }
    static abstract T One { get; }
    static abstract int BitWidth { get; }
    abstract static T FromValue(bool value);
    abstract static T FromBits(bool[] bits);

    abstract static implicit operator T(bool value);
    abstract static implicit operator T(int value);
}

public interface IByteValue<T> : IValue<T> where T : struct, IByteValue<T>, IValue<T>
{
    Bit LastBit();

    abstract static implicit operator T(Bit value);
    abstract static implicit operator int(T value);
    virtual static implicit operator Bit(T value) => value.GetBit(0);
}

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
