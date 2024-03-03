using Moshaveran.Library.Internals.Results;

using System.Diagnostics.CodeAnalysis;

namespace Moshaveran.Library.Results;

public interface IResult
{
    [NotNull]
    static IResult Failed => Result.Failed;

    [NotNull]
    static IResult Succeed => Result.Succeed;

    [NotNull]
    IEnumerable<Exception> Exceptions { get; }

    bool IsFailure { get; }
    bool IsSucceed { get; }
    string? Message { get; }

    [return: NotNull]
    static IResult Create(bool succeed)
        => Result.Create(succeed);

    [return: NotNull]
    static IResult Fail(Exception exception)
        => Result.Fail(exception);

    [return: NotNull]
    static IResult<TValue> Fail<TValue>(TValue value, Exception exception)
        => Result.Fail(value, exception);

    [return: NotNull]
    static IResult<TValue> Fail<TValue>(TValue value)
        => Result.Fail(value);

    [return: NotNull]
    static IResult<TValue?> Fail<TValue>()
        => Result.Fail<TValue>();

    [return: NotNull]
    static IResult Fail()
        => Result.Fail();

    [return: NotNull]
    static IResult Fail(string message)
        => Result.Fail(message);

    [return: NotNull]
    static IResult<TValue?> Fail<TValue>(string message)
        => Result.Fail<TValue>(message);

    [return: NotNull]
    static IResult<TValue> Fail<TValue>(TValue value, string message)
        => Result.Fail(value, message);

    [return: NotNull]
    static IResult Success()
        => Result.Success();

    [return: NotNull]
    static IResult Success(string message)
        => Result.Success(message);

    [return: NotNull]
    static IResult<TValue> Success<TValue>(TValue value, string message)
        => Result.Success(value, message);

    [return: NotNull]
    static IResult<TValue> Success<TValue>(TValue value)
        => Result.Success(value);
}