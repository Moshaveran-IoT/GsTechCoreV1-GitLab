using Moshaveran.Library.Results;
using Moshaveran.Library.Validations;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Moshaveran.Library;

[DebuggerStepThrough]
[StackTraceHidden]
public static class CodeHelper
{
    public static IResult CatchResult(Action action)
    {
        Check.MustBeArgumentNotNull(action);
        try
        {
            action();
            return IResult.Success();
        }
        catch (Exception ex)
        {
            return IResult.Fail(ex);
        }
    }

    public static async ValueTask<IResult<TResult?>> CatchResultAsync<TResult>(Func<Task<TResult?>> action)
    {
        Check.MustBeArgumentNotNull(action);
        try
        {
            var result = await action();
            return IResult.Success(result);
        }
        catch (Exception ex)
        {
            return IResult.Fail<TResult>(ex);
        }
    }

    public static async ValueTask<IResult<TResult>> CatchResult<TResult>(Func<ValueTask<TResult>> action)
    {
        Check.MustBeArgumentNotNull(action);
        try
        {
            var result = await action();
            return IResult.Success(result);
        }
        catch (Exception ex)
        {
            return IResult.Fail<TResult>(ex)!;
        }
    }

    public static IResult<TResult?> CatchResult<TResult>(Func<TResult?> action)
    {
        Check.MustBeArgumentNotNull(action);
        try
        {
            var result = action();
            return IResult.Success(result);
        }
        catch (Exception ex)
        {
            return IResult.Fail<TResult>(ex);
        }
    }

    public static T? New<T>()
        where T : new() => new();

    [return: NotNullIfNotNull(nameof(o))]
    public static T? With<T>(this T? o, in Action<T?> action)
    {
        action?.Invoke(o);
        return o;
    }

    [return: NotNullIfNotNull(nameof(o))]
    public static T? With<T>(this T? o, in Func<T?, T?> action)
    {
        var result = o;
        if (action != null)
        {
            result = action(result);
        }

        return result;
    }
}