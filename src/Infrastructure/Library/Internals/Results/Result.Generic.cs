using Moshaveran.Library.Results;

using System.Diagnostics;

namespace Moshaveran.Library.Internals.Results;

[DebuggerStepThrough, StackTraceHidden]
internal sealed class Result<TValue>(TValue value, bool isSucceed, string? message = null, IResult? innerResult = null, params Exception[] exceptions)
    : ResultBase(isSucceed, message, exceptions, innerResult), IResult<TValue>
{
    private static Result<TValue?>? _failed;
    private static Result<TValue?>? _succeed;

    public Result(TValue value, bool isSucceed, string? message = null, IEnumerable<Exception>? exceptions = null, IResult? innerResult = null)
        : this(value, isSucceed, message, innerResult, exceptions?.ToArray() ?? [])
    {
    }

    public static Result<TValue?> Failed => _failed ??= new(default, false);

    public static Result<TValue?> Succeed => _succeed ??= new(default, true);

    public TValue Value { get; } = value;
}