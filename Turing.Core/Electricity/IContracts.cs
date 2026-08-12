namespace Turing.Core.Electricity;

public interface IBitValue;

public interface IBitValue<T> : IBitValue where T : struct, IBitValue<T>
{
    T Value { get; }
    static abstract int BitWidth { get; }
    T FromValue(bool value);
    T FromBits(bool[] bits);
    Bit GetBit(int index);
    T SetBit(int index, bool value);
}

public interface IGate<TInput, TOutput> where TInput : IBitValue where TOutput : IBitValue;

public interface IUnaryGate<TInput, TOutput> where TInput : IBitValue where TOutput : IBitValue
{
    TOutput Evaluate(TInput a);
}

public interface IStateGate<T> where T : struct, IBitValue<T>
{
    T State { get; }
    void Reset();
}
