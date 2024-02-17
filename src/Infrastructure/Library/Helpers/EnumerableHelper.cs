using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Moshaveran.Library.Helpers;

[DebuggerStepThrough]
[StackTraceHidden]
public static class EnumerableHelper
{
    [return: NotNullIfNotNull(nameof(values))]
    public static IEnumerable<T>? Build<T>(this IEnumerable<T>? values)
        => values?.ToImmutableArray().AsEnumerable();

    public static IEnumerable<T> Combine<T>(this IEnumerable<IEnumerable<T>> items)
        => items.SelectMany(x => x);

    /// <summary>
    /// Returns an IEnumerable of non-null elements from the given IEnumerable of nullable elements.
    /// </summary>
    [return: NotNull]
    public static IEnumerable<TSource> Compact<TSource>(this IEnumerable<TSource?>? items) where TSource : class =>
        items?
             .Where([DebuggerStepThrough] (x) => x != null)
             .Select([DebuggerStepThrough] (x) => x!)
        ?? [];

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

    public static IEnumerable<T> ToEnumerable<T>(T item)
    {
        yield return item;
    }
}