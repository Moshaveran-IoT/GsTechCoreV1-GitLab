using Moshaveran.Library.Results.Internals;

namespace Moshaveran.Library.Results;

public sealed class Result : ResultBase, IResult
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