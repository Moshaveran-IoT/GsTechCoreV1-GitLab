﻿using Moshaveran.Library.Results;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Moshaveran.Library.Internals.Results;

[DebuggerStepThrough, StackTraceHidden]
internal sealed record Result<TValue>(TValue Value, bool IsSucceed, string? Message = null, IEnumerable<Exception>? Exceptions = null, IResult? InnerResult = null)
    : IResult<TValue>
{
    public Result(TValue Value, IResult result)
        : this(Value, result.IsSucceed, result.Message, result.Exceptions, result.InnerResult)
    {
    }

    private static IResult<TValue?>? _failed;
    private static IResult<TValue?>? _succeed;

    [NotNull]
    public static IResult<TValue?> Failed => _failed ??= IResult.Fail<TValue?>();

    [NotNull]
    public static IResult<TValue?> Succeed => _succeed ??= IResult.Success<TValue?>();

    public Exception? Exception => this.Exceptions?.SingleOrDefault();
}