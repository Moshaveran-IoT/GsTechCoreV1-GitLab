using System.Diagnostics;

namespace Moshaveran.Infrastructure.Helpers;

[DebuggerStepThrough]
[StackTraceHidden]
public static class EnumerableHelper
{
    public static async Task Enumerate<T>(this IEnumerable<T> values, Func<T, CancellationToken, Task> action, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(action);
        if (values?.Any() != true)
        {
            return;
        }

        using var enumerator = values.GetEnumerator();
        while (enumerator.MoveNext() && !token.IsCancellationRequested)
        {
            await action(enumerator.Current, token);
        }
    }
}