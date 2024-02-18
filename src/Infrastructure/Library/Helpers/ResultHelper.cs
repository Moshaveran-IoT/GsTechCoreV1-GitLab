﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Moshaveran.Library.Results;
using Moshaveran.Library.Validations;

namespace Moshaveran.Library.Helpers;

[DebuggerStepThrough]
[StackTraceHidden]
public static class ResultHelper
{
    public static void Deconstruct<TValue>(this Result<TValue?> r, out Result result, out TValue? value)
        => (result, value) = (r, r.Value);

    public static async Task<TValue> GetValueAsync<TValue>(this Task<IResult<TValue>> result)
        => (await result.ArgumentNotNull()).Value;

    [return: NotNullIfNotNull(nameof(result))]
    public static TResult? OnFailure<TResult>(this TResult? result, Action<TResult> action)
        where TResult : IResult
    {
        if (result?.IsSucceed != true)
        {
            action.ArgumentNotNull()(result!);
        }

        return result!;
    }

    public static async Task<TFuncResult?> OnFailure<TResult, TFuncResult>(this Task<TResult?> resultAsync, Func<TResult, TFuncResult> action, TFuncResult defaultFuncResult = default!)
        where TResult : IResult
    {
        Check.MustBeArgumentNotNull(resultAsync);

        var result = await resultAsync;
        return result?.IsSucceed != true
            ? action.ArgumentNotNull()(result!)
            : defaultFuncResult;
    }

    [return: NotNullIfNotNull(nameof(result))]
    public static TResult OnSucceed<TResult>(this TResult result, Action<TResult> action)
        where TResult : IResult
    {
        if (result?.IsSucceed == true)
        {
            action.ArgumentNotNull()(result);
        }

        return result!;
    }

    public static async Task<TResult> OnSucceed<TResult>(this Task<TResult> resultAsync, Action<TResult> action)
        where TResult : IResult
    {
        Check.MustBeArgumentNotNull(resultAsync);

        var result = await resultAsync;
        if (result?.IsSucceed == true)
        {
            action.ArgumentNotNull()(result);
        }

        return result!;
    }

    public static TFuncResult OnSucceed<TResult, TFuncResult>(this TResult result, Func<TResult, TFuncResult> action, TFuncResult defaultFuncResult = default!)
        where TResult : IResult
    {
        return result?.IsSucceed == true
            ? action.ArgumentNotNull()(result)
            : defaultFuncResult;
    }

    public static async Task<TFuncResult> OnSucceedAsync<TResult, TFuncResult>(this Task<TResult> resultAsync, Func<TResult, TFuncResult> action, TFuncResult defaultFuncResult = default!)
        where TResult : IResult
    {
        Check.MustBeArgumentNotNull(resultAsync);

        var result = await resultAsync;
        return result?.IsSucceed == true
            ? action.ArgumentNotNull()(result)
            : defaultFuncResult;
    }

    public static async Task<TFuncResult> Process<TResult, TFuncResult>(this Task<TResult?> resultAsync, Func<TResult, TFuncResult> onSucceed, Func<TResult?, TFuncResult> onFailure)
        where TResult : IResult
    {
        var result = await resultAsync;
        return result?.IsSucceed == true
            ? onSucceed.ArgumentNotNull()(result)
            : onFailure.ArgumentNotNull()(result);
    }

    public static TFuncResult Process<TResult, TFuncResult>(this TResult? result, Func<TResult, TFuncResult> onSucceed, Func<TResult?, TFuncResult> onFailure)
        where TResult : IResult
        => result?.IsSucceed == true
            ? onSucceed.ArgumentNotNull()(result)
            : onFailure.ArgumentNotNull()(result);

    public static Task<IResult> ToAsync(this IResult result)
        => Task.FromResult(result);

    public static Task<IResult<TValue>> ToAsync<TValue>(this IResult<TValue> result)
        => Task.FromResult(result);

    public static IResult<TValue> WithValue<TValue>(this IResult result, TValue value)
        => new Result<TValue>(value, result.IsSucceed, result.Message, result.Exceptions)!;
}