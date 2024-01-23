using System.Diagnostics;

namespace Moshaveran.Infrastructure.Coding;

public static class CodeHelper
{
    [DebuggerStepThrough]
    [StackTraceHidden]
    public static T With<T>(this T o, in Action<T> action)
    {
        action?.Invoke(o);
        return o;
    }

    [DebuggerStepThrough]
    [StackTraceHidden]
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