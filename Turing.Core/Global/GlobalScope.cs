global using Turing.Core.Global;
global using static Turing.Core.Global.GlobalScope;
global using Bit = Turing.Core.Electricity.Bit;
global using Byte = Turing.Core.Electricity.Byte;
global using Long = Turing.Core.Electricity.Long;
global using Short = Turing.Core.Electricity.Short;
using Turing.Core.Electricity;

namespace Turing.Core.Global;

public static partial class GlobalScope
{
    public static T ExecTo<T>(TurComponentValue value)
        where T : struct, IValue<T>
    {
        return value.Into<T>();
    }
}
