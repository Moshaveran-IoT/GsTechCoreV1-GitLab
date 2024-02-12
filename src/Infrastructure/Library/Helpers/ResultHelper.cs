using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Moshaveran.Library.Results;
using Moshaveran.Library.Validations;

namespace Moshaveran.Library.Helpers;

[DebuggerStepThrough]
[StackTraceHidden]
public static class ResultHelper
{
    [return: NotNullIfNotNull(nameof(result))]
    public static TResult OnFailure<TResult>(this TResult result, Action<TResult> action)
        where TResult : ResultBase
    {
        if (result?.IsSucceed != true)
        {
            action.ArgumentNotNull()(result!);
        }

        return result!;
    }

    public static async Task<TFuncResult> OnFailure<TResult, TFuncResult>(this Task<TResult> resultAsync, Func<TResult, TFuncResult> action, TFuncResult defaultFuncResult = default!)
        where TResult : ResultBase
    {
        var result = await resultAsync;
        return result?.IsSucceed != true
            ? action.ArgumentNotNull()(result!)
            : defaultFuncResult;
    }

    [return: NotNullIfNotNull(nameof(result))]
    public static TResult OnSucceed<TResult>(this TResult result, Action<TResult> action)
        where TResult : ResultBase
    {
        if (result?.IsSucceed == true)
        {
            action.ArgumentNotNull()(result);
        }

        return result!;
    }

    public static async Task<TResult> OnSucceed<TResult>(this Task<TResult> resultAsync, Action<TResult> action)
        where TResult : ResultBase
    {
        var result = await resultAsync;
        if (result?.IsSucceed == true)
        {
            action.ArgumentNotNull()(result);
        }

        return result!;
    }

    public static async Task<TFuncResult> OnSucceed<TResult, TFuncResult>(this Task<TResult> resultAsync, Func<TResult, TFuncResult> action, TFuncResult defaultFuncResult = default!)
        where TResult : ResultBase
    {
        var result = await resultAsync;
        return result?.IsSucceed == true
            ? action.ArgumentNotNull()(result)
            : defaultFuncResult;
    }

    public static async Task<TFuncResult> Process<TResult, TFuncResult>(this Task<TResult> resultAsync, Func<TResult, TFuncResult> onSucceed, Func<TResult, TFuncResult> onFailure)
        where TResult : ResultBase
    {
        var result = await resultAsync;
        return result?.IsSucceed == true
            ? onSucceed.ArgumentNotNull()(result!)
            : onFailure.ArgumentNotNull()(result!);
    }

    public static TFuncResult Process<TResult, TFuncResult>(this TResult result, Func<TResult, TFuncResult> onSucceed, Func<TResult, TFuncResult> onFailure)
        where TResult : ResultBase
        => result?.IsSucceed == true
            ? onSucceed.ArgumentNotNull()(result!)
            : onFailure.ArgumentNotNull()(result!);

    public static Task<TResult> ToAsync<TResult>(this TResult result)
        => Task.FromResult(result);
}