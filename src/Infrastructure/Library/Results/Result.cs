using Moshaveran.Library.Results.Internals;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Moshaveran.Library.Results;

[DebuggerStepThrough, StackTraceHidden]
public sealed class Result : ResultBase, IResult
{
    private static IResult? _failed;

    private static IResult? _succeed;

    internal Result(bool isSucceed, string? message = null, params Exception[] exceptions) : base(isSucceed, message, exceptions)
    {
    }

    internal Result(bool isSucceed, string? message = null, IEnumerable<Exception>? exceptions = null)
        : this(isSucceed, message, exceptions?.ToArray() ?? [])
    {
    }

    [NotNull]
    public static IResult Failed => _failed ??= Fail();

    [NotNull]
    public static IResult Succeed => _succeed ??= Success();

    [return: NotNull]
    public static IResult Create(bool isSucceed)
        => new Result(isSucceed);

    [return: NotNull]
    public static IResult Create(bool isSucceed, string? message)
        => new Result(isSucceed, message);

    [return: NotNull]
    public static IResult Create(Exception exception)
        => new Result(false, exceptions: exception);

    [return: NotNull]
    public static IResult<TValue> Create<TValue>(TValue value, bool isSucceed)
        => new Result<TValue>(value, isSucceed);

    [return: NotNull]
    public static IResult<TValue> Create<TValue>(TValue value, bool isSucceed, string? message)
        => new Result<TValue>(value, isSucceed, message);

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
    public static IResult Fail(Exception exception)
        => new Result(false, exceptions: exception);

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
}