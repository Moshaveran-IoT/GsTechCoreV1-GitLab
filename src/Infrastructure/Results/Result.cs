using System.Diagnostics.CodeAnalysis;

namespace Moshaveran.Infrastructure.Results;

public sealed class Result : ResultBase
{
    private static Result? _failed;

    private static Result? _succeed;

    private Result(bool isSucceed)
        : base(isSucceed)
    {
    }

    public static Result Failed => _failed ??= Create(true);

    public static Result Succeed => _succeed ??= Create(true);

    public static Result<TValue> CreateSucceed<TValue>(TValue value)
        => InnerCreate(value, true);
    public static Result<TValue> CreateFailure<TValue>(TValue value)
        => InnerCreate(value, false);

    public static Result Create(bool isSucceed)
        => new(isSucceed);

    public static Result Create(bool isSucceed, string? message)
        => InnerCreate(isSucceed, message);

    public static Result Create(Exception? exception)
        => InnerCreate(false, exception: exception);

    public static Result Create<TValue>(Result<TValue> result)
        => Create(result.IsSucceed, result.Message);

    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed)
        => InnerCreate(value, isSucceed);

    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed, Exception? exception)
        => InnerCreate(value, isSucceed, exception: exception);

    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed, string? message)
        => InnerCreate(value, isSucceed, message);

    public static Result Create(bool isSucceed, Exception? exception)
        => InnerCreate(isSucceed, exception: exception);

    public static Result<TValue> Create<TValue>(TValue value, Result result)
        => Create(value, result.IsSucceed, result.Message);

    public Result WithMessage([DisallowNull] string message)
        => InnerCreate(this.IsSucceed, message, this.Exception);

    private static Result InnerCreate(bool isSucceed, string? message = null, Exception? exception = null)
        => new(isSucceed)
        {
            Exception = exception,
            Message = message,
        };

    private static Result<TValue> InnerCreate<TValue>(TValue value, bool isSucceed, string? message = null, Exception? exception = null)
        => new(value, isSucceed)
        {
            Exception = exception,
            Message = message,
        };
}

public sealed class Result<TValue> : ResultBase
{
    private static Result<TValue?>? _failed;
    private static Result<TValue?>? _succeed;

    internal Result(TValue? value, bool isSucceed)
        : base(isSucceed) => this.Value = value;

    public static Result<TValue?> Failed => _failed ??= Result.Create<TValue?>(default, false);
    public static Result<TValue?> Succeed => _succeed ??= Result.Create<TValue?>(default, true);

    public TValue? Value { get; }

    public static implicit operator Result(Result<TValue?> result)
        => Result.Create(result);

    public static implicit operator TValue?(Result<TValue?> result)
        => result.Value;
}

public abstract class ResultBase
{
    protected ResultBase(bool isSucceed)
        => this.IsSucceed = isSucceed;

    protected ResultBase(bool isSucceed, string? message)
        => (this.IsSucceed, this.Message) = (isSucceed, message);

    protected ResultBase(bool isSucceed, Exception? exception)
        => (this.IsSucceed, this.Exception) = (isSucceed, exception);

    public Exception? Exception { get; set; }

    public bool IsFailure => !this.IsSucceed;

    public bool IsSucceed { get; init; }

    public string? Message { get; init; }

    public object? State => (object?)Exception ?? Message;
}