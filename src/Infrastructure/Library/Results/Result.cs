using Moshaveran.Library.Results.Internals;

namespace Moshaveran.Library.Results;

public sealed class Result : ResultBase
{
    private static Result? _failed;

    private static Result? _succeed;

    public Result(bool isSucceed, string? message = null, params Exception[] exceptions) : base(isSucceed, message, exceptions)
    {
    }

    public Result(bool isSucceed, string? message = null, IEnumerable<Exception>? exceptions = null) : base(isSucceed, message, exceptions)
    {
    }

    public static Result Failed => _failed ??= new(true);

    public static Result Succeed => _succeed ??= new(true);

    public static Result Create(bool isSucceed)
        => new(isSucceed);

    public static Result Create(bool isSucceed, string? message)
        => new(isSucceed, message);

    public static Result Create(Exception exception)
        => new(false, exceptions: exception);

    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed)
        => new(value, isSucceed);

    public static Result<TValue> Create<TValue>(TValue value, bool isSucceed, string? message)
        => new(value, isSucceed, message);

    public static Result<TValue> CreateFailure<TValue>(TValue value)
        => new(value, false);

    public static Result CreateFailure(Exception exception)
        => new(false, exceptions: exception);

    public static Result<TValue> CreateFailure<TValue>(TValue value, string message)
        => new(value, false, message);

    public static Result<TValue> CreateSucceed<TValue>(TValue value)
        => new(value, true);

    public Result<TValue> WithValue<TValue>(TValue value)
        => new Result<TValue>(value, this.IsSucceed, this.Message, this.Exceptions)!;
}

public sealed class Result<TValue> : ResultBase
{
    private static Result<TValue>? _failed;
    private static Result<TValue>? _succeed;

    public Result(TValue value, bool isSucceed, string? message = null, params Exception[] exceptions) : base(isSucceed, message, exceptions)
        => this.Value = value;

    public Result(TValue value, bool isSucceed, string? message = null, IEnumerable<Exception>? exceptions = null) : base(isSucceed, message, exceptions)
        => this.Value = value;

    public static Result<TValue> Failed => _failed ??= new(default!, false);
    public static Result<TValue> Succeed => _succeed ??= new(default!, true);

    public TValue Value { get; }

    public static implicit operator (Result Result, TValue? Value)(Result<TValue?> result)
        => (result, result.Value);

    public static implicit operator Result(Result<TValue?> result)
        => new(result.IsSucceed, result.Message, result.Exceptions);

    public static implicit operator TValue?(Result<TValue?> result)
        => result.Value;

    public Result<TValue> WithValue(TValue value)
        => new(value, this.IsSucceed, this.Message, this.Exceptions);

    public Result<TValue1> WithValue<TValue1>(TValue1 value)
        => new(value, this.IsSucceed, this.Message, this.Exceptions);
}