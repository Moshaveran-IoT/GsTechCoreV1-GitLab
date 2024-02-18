using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Moshaveran.Library;

[DebuggerStepThrough]
[StackTraceHidden]
public static class CodeHelper
{
    public static T New<T>()
        where T : new() => new();

    [return: NotNullIfNotNull(nameof(o))]
    public static T With<T>(this T o, in Action<T> action)
    {
        action?.Invoke(o);
        return o;
    }

    [return: NotNullIfNotNull(nameof(o))]
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