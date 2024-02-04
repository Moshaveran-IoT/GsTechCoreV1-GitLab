using System.Diagnostics;

namespace Moshaveran.Infrastructure.Coding;

[DebuggerStepThrough]
[StackTraceHidden]
public static class CodeHelper
{
    public static T With<T>(this T o, in Action<T> action)
    {
        action?.Invoke(o);
        return o;
    }

    public static T With<T>(this T o, in Func<T, T> action)
    {
        var result = o;
        if (action != null)
        {
            result = action(result);
        }

        return result;
    }
}