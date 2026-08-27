namespace Turing.Core.Executeables.Symphony;

public class LabelToken
{
    private int? _value;
    private Action<int>? _callback;

    public int RequireLabel(Action<int> value)
    {
        if (_value is { } x) return x;
        _callback += value;
        return 0;
    }

    public void DefineLabel(int value)
    {
        (var callback, _callback) = (_callback, null);
        if (callback is { }) callback(value);
        _value = value;
    }
}
