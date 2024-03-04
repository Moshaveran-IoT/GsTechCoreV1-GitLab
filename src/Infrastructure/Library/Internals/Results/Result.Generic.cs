using Moshaveran.Library.Results;

using System.Diagnostics;

namespace Moshaveran.Library.Internals.Results;

[DebuggerStepThrough, StackTraceHidden]
internal sealed class Result<TValue>(TValue value, bool isSucceed, string? message = null, IResult? innerResult = null, params Exception[] exceptions)
    : ResultBase(isSucceed, message, exceptions, innerResult), IResult<TValue>
{
    private static Result<TValue?>? _failed;
    private static Result<TValue?>? _succeed;

    internal Result(TValue value, bool isSucceed, string? message = null, IEnumerable<Exception>? exceptions = null, IResult? innerResult = null)
        : this(value, isSucceed, message, innerResult, exceptions?.ToArray() ?? [])
    {
    }

    internal Result(IResult<TValue> result)
        : this(result.Value, result.IsSucceed, result.Message, result.Exceptions, result.InnerResult)
    {
    }

    internal Result(in TValue value, in IResult result)
        : this(value, result.IsSucceed, result.Message, result.Exceptions, result.InnerResult)
    {
    }

    public static Result<TValue?> Failed => _failed ??= new(default, false);

    public static Result<TValue?> Succeed => _succeed ??= new(default, true);

    public TValue Value { get; } = value;
}