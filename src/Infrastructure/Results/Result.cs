using System.Diagnostics.CodeAnalysis;

namespace Moshaveran.Infrastructure.Results;

public sealed class Result(bool isSucceed, string? message = null, Exception? exception = null) : ResultBase(isSucceed, message, exception)
{
    private static Result? _failed;

    private static Result? _succeed;

    public static Result Failed => _failed ??= new(true);

    public static Result Succeed => _succeed ??= new(true);

    public static Result Create(bool isSucceed)
        => new(isSucceed);

    public static Result Create(bool isSucceed, string? message)
        => new(isSucceed, message);

    public static Result Create(Exception? exception)
        => new(false, exception: exception);

    public static Result<TValue> Create<TValue>(Result<TValue> result)
        => new(result.Value, result.IsSucceed, result.Message, result.Exception);

    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed)
        => new(value, isSucceed);

    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed, Exception? exception)
        => new(value, isSucceed, exception: exception);

    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed, string? message)
        => new(value, isSucceed, message);

    public static Result Create(bool isSucceed, Exception? exception)
        => new(isSucceed, exception: exception);

    public static Result<TValue> Create<TValue>(TValue value, Result result)
        => new(value, result.IsSucceed, result.Message);

    public static Result<TValue> CreateFailure<TValue>(TValue value)
        => new(value, false);

    public static Result<TValue> CreateFailure<TValue>(TValue value, string message)
        => new(value, false, message);

    public static Result<TValue> CreateSucceed<TValue>(TValue value)
        => new(value, true);

    public static Result<TValue1> CreateWitValue<TValue1>(Result<TValue1> result, TValue1 value)
        => new(value, result.IsSucceed, result.Message, result.Exception);

    public static Result<TValue2> CreateWitValue<TValue1, TValue2>(Result<TValue1> result, TValue2 value)
        => new(value, result.IsSucceed, result.Message, result.Exception);

    public Result WithMessage([DisallowNull] string message)
        => new(this.IsSucceed, message, this.Exception);
}

public sealed class Result<TValue>(TValue value, bool isSucceed, string? message = null, Exception? exception = null) : ResultBase(isSucceed, message, exception)
{
    private static Result<TValue>? _failed;
    private static Result<TValue>? _succeed;

    public static Result<TValue> Failed => _failed ??= new(default!, false);
    public static Result<TValue> Succeed => _succeed ??= new(default!, true);

    public TValue Value { get; } = value;

    public static implicit operator Result(Result<TValue?> result)
        => new(result.IsSucceed, result.Message, result.Exception);

    public static implicit operator TValue?(Result<TValue?> result)
        => result.Value;

    public Result<TValue> WithValue(TValue value)
        => new(value, this.IsSucceed, this.Message, this.Exception);

    public Result<TValue1> WithValue<TValue1>(TValue1 value)
        => new(value, this.IsSucceed, this.Message, this.Exception);
}

public abstract class ResultBase(bool isSucceed, string? message = null, Exception? exception = null)
{
    public Exception? Exception { get; } = exception;

    public bool IsFailure => !this.IsSucceed;

    public bool IsSucceed { get; } = isSucceed;

    public string? Message { get; } = message;

    public object? State => (object?)Exception ?? Message;
}