using Moshaveran.Library.Exceptions;
using Moshaveran.Library.Results.Internals;

namespace Moshaveran.Library.Results;

public sealed class Result<TValue> : ResultBase, IResult, IResult<TValue>
{
    private static Result<TValue>? _failed;
    private static Result<TValue>? _succeed;

    public Result(TValue value, bool isSucceed, string? message = null, params IGsTechException[] exceptions) : base(isSucceed, message, exceptions)
        => this.Value = value;

    public Result(TValue value, bool isSucceed, string? message = null, IEnumerable<IGsTechException>? exceptions = null) : base(isSucceed, message, exceptions)
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