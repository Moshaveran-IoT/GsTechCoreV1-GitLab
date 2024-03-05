using Moshaveran.Library.Internals.Results;

using System.Diagnostics.CodeAnalysis;

namespace Moshaveran.Library.Results;

public interface IResult
{
    [NotNull]
    static IResult Failed => Result.Failed;

    [NotNull]
    static IResult Succeed => Result.Succeed;

    IEnumerable<Exception>? Exceptions { get; }

    IResult? InnerResult { get; }

    bool IsFailure => !this.IsSucceed;
    bool IsSucceed { get; }
    string? Message { get; }

    [return: NotNull]
    static IResult Create(bool succeed)
        => new Result(succeed);

    [return: NotNull]
    static IResult Fail(Exception exception)
        => new Result(false) { Exceptions = [exception] };

    [return: NotNull]
    static IResult<TValue> Fail<TValue>(TValue value, Exception exception)
        => new Result<TValue>(value, false) { Exceptions = [exception] };

    [return: NotNull]
    static IResult<TValue?> Fail<TValue>(Exception exception)
        => new Result<TValue?>(default, false) { Exceptions = [exception] };

    [return: NotNull]
    static IResult<TValue> Fail<TValue>(TValue value)
        => new Result<TValue>(value, false);

    [return: NotNull]
    static IResult<TValue?> Fail<TValue>()
        => new Result<TValue?>(default, false);

    [return: NotNull]
    static IResult Fail()
        => new Result(false);

    [return: NotNull]
    static IResult Fail(string message)
        => new Result(false, message);

    [return: NotNull]
    static IResult<TValue?> Fail<TValue>(string message)
        => new Result<TValue?>(default, false, message);

    [return: NotNull]
    static IResult<TValue> Fail<TValue>(TValue value, string message)
        => new Result<TValue>(value, false, message);

    [return: NotNull]
    static IResult Success()
        => new Result(true);

    [return: NotNull]
    static IResult Success(string message)
        => new Result(true, Message: message);

    [return: NotNull]
    static IResult<TValue?> Success<TValue>()
        => new Result<TValue?>(default, true);

    [return: NotNull]
    static IResult<TValue> Success<TValue>(TValue value, string message)
        => new Result<TValue>(value, true, Message: message);

    [return: NotNull]
    static IResult<TValue> Success<TValue>(TValue value)
        => new Result<TValue>(value, true);
}