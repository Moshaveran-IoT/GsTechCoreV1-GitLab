namespace Moshaveran.Infrastructure.Results;

public sealed class Result : ResultBase
{
    private static Result? _failed;
    private static Result? _succeed;

    private Result(bool isSucceed, string? message) : base(isSucceed, message)
    {
    }

    private Result(bool isSucceed, Exception? exception) : base(isSucceed, exception)
    {
    }

    private Result(bool isSucceed)
        : base(isSucceed)
    {
    }

    public static Result Failed => _failed ??= Create(true);

    public static Result Succeed => _succeed ??= Create(true);

    public static Result Create(bool isSucceed)
        => new(isSucceed);

    public static Result Create(bool isSucceed, string? message)
        => new(isSucceed, message);

    public static Result Create(Exception? exception)
        => new(false, exception);

    public static Result Create<TValue>(Result<TValue> result)
        => Create(result.IsSucceed, result.Message);

    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed)
        => new(value, isSucceed);

    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed, Exception? exception)
        => new(value, isSucceed, exception);

    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed, string? message)
        => new(value, isSucceed, message);

    public static Result Create(bool isSucceed, Exception? exception)
        => new(isSucceed, exception);

    public static Result<TValue> Create<TValue>(TValue value, Result result)
        => Create(value, result.IsSucceed, result.Message);
}

public sealed class Result<TValue> : ResultBase
{
    private static Result<TValue?>? _failed;
    private static Result<TValue?>? _succeed;

    internal Result(TValue? value, bool isSucceed)
        : base(isSucceed) => this.Value = value;

    internal Result(TValue? value, bool isSucceed, string? message) : base(isSucceed, message) => this.Value = value;

    internal Result(TValue? value, bool isSucceed, Exception? exception) : base(isSucceed, exception) => this.Value = value;

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