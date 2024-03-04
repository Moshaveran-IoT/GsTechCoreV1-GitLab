using Moshaveran.Library.Results;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Moshaveran.Library.Internals.Results;

[DebuggerStepThrough, StackTraceHidden]
internal sealed class Result(bool isSucceed, string? message = null, IResult? innerResult = null, params Exception[] exceptions)
    : ResultBase(isSucceed, message, exceptions, innerResult), IResult
{
    private static IResult? _failed;

    private static IResult? _succeed;

    internal Result(bool isSucceed, string? message = null, IEnumerable<Exception>? exceptions = null, IResult? innerResult = null)
        : this(isSucceed, message, innerResult, exceptions?.ToArray() ?? [])
    {
    }

    internal Result(IResult result)
        : this(result.IsSucceed, result.Message, result.Exceptions, result.InnerResult)
    { }

    [NotNull]
    public static IResult Failed => _failed ??= Fail();

    [NotNull]
    public static IResult Succeed => _succeed ??= Success();

    [return: NotNull]
    public static IResult Fail(Exception exception)
        => new Result(false, exceptions: exception);

    [return: NotNull]
    public static IResult<TValue> Fail<TValue>(TValue value, Exception exception)
        => new Result<TValue>(value, false, exceptions: exception);

    [return: NotNull]
    public static IResult<TValue?> Fail<TValue>(Exception exception)
        => new Result<TValue?>(default, false, exceptions: exception);

    [return: NotNull]
    public static IResult<TValue> Fail<TValue>(TValue value)
        => new Result<TValue>(value, false);

    [return: NotNull]
    public static IResult<TValue?> Fail<TValue>()
        => new Result<TValue?>(default, false);

    [return: NotNull]
    public static IResult Fail()
        => new Result(false);

    [return: NotNull]
    public static IResult Fail(string message)
        => new Result(false, message: message);

    [return: NotNull]
    public static IResult<TValue?> Fail<TValue>(string message)
        => new Result<TValue?>(default, false, message);

    [return: NotNull]
    public static IResult<TValue> Fail<TValue>(TValue value, string message)
        => new Result<TValue>(value, false, message);

    [return: NotNull]
    public static IResult Success(string message)
        => new Result(true, message: message);

    [return: NotNull]
    public static IResult Success()
        => new Result(true);

    [return: NotNull]
    public static IResult<TValue> Success<TValue>(TValue value, string message)
        => new Result<TValue>(value, true, message: message);

    [return: NotNull]
    public static IResult<TValue> Success<TValue>(TValue value)
        => new Result<TValue>(value, true);

    [return: NotNull]
    internal static IResult Create(bool succeed)
        => new Result(succeed);
}