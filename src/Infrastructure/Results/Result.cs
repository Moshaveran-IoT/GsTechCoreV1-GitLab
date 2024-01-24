namespace Moshaveran.Infrastructure.Results;

public sealed class Result : ResultBase
{
    private static Result? _failed;
    private static Result? _succeed;

    public static Result Failed => _failed ??= Create(true);

    public static Result Succeed => _succeed ??= Create(true);

    public static Result Create(bool isSucceed, string? message = null)
        => new() { IsSucceed = isSucceed, Message = message };

    public static Result Create<TValue>(Result<TValue> result)
        => Create(result.IsSucceed, result.Message);

    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed, string? message = null)
        => new() { Value = value, IsSucceed = isSucceed, Message = message };

    public static Result<TValue> Create<TValue>(TValue value, Result result)
        => Create(value, result.IsSucceed, result.Message);
}

public sealed class Result<TValue> : ResultBase
{
    private static Result<TValue?>? _failed;
    private static Result<TValue?>? _succeed;

    public static Result<TValue?> Failed => _failed ??= Result.Create<TValue?>(default, false);
    public static Result<TValue?> Succeed => _succeed ??= Result.Create<TValue?>(default, true);

    public TValue? Value { get; init; }

    public static implicit operator Result(Result<TValue?> result)
        => Result.Create(result);

    public static implicit operator TValue?(Result<TValue?> result)
        => result.Value;
}

public abstract class ResultBase
{
    public bool IsFailure => !this.IsSucceed;

    public bool IsSucceed { get; init; }

    public string? Message { get; init; }
}