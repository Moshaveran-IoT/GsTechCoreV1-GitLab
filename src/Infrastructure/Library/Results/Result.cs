using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using Moshaveran.Library.Results.Internals;

namespace Moshaveran.Library.Results;

[DebuggerStepThrough]
[StackTraceHidden]
public sealed class Result : ResultBase, IResult
{
    private static Result? _failed;

    private static Result? _succeed;

    internal Result(bool isSucceed, string? message = null, params Exception[] exceptions) : base(isSucceed, message, exceptions)
    {
    }

    internal Result(bool isSucceed, string? message = null, IEnumerable<Exception>? exceptions = null) : base(isSucceed, message, exceptions)
    {
    }

    [NotNull]
    public static Result Failed => _failed ??= new(false);

    [NotNull]
    public static Result Succeed => _succeed ??= new(true);

    [return: NotNull]
    public static Result Create(bool isSucceed)
        => new(isSucceed);

    [return: NotNull]
    public static Result Create(bool isSucceed, string? message)
        => new(isSucceed, message);

    [return: NotNull]
    public static Result Create(Exception exception)
        => new(false, exceptions: exception);

    [return: NotNull]
    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed)
        => new(value, isSucceed);

    [return: NotNull]
    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed, string? message)
        => new(value, isSucceed, message);

    [return: NotNull]
    public static Result<TValue> Fail<TValue>(TValue value)
        => new(value, false);

    [return: NotNull]
    public static Result Fail(Exception exception)
        => new(false, exceptions: exception);

    [return: NotNull]
    public static Result<TValue> Fail<TValue>(TValue value, string message)
        => new(value, false, message);

    [return: NotNull]
    public static Result<TValue> Success<TValue>(TValue value)
        => new(value, true);
}