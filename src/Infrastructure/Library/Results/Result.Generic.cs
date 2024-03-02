using System.Diagnostics;

using Moshaveran.Library.Results.Internals;

namespace Moshaveran.Library.Results;

[DebuggerStepThrough, StackTraceHidden]
public sealed class Result<TValue> : ResultBase, IResult<TValue>
{
    private static Result<TValue?>? _failed;
    private static Result<TValue?>? _succeed;

    public Result(TValue value, bool isSucceed, string? message = null, params Exception[] exceptions) : base(isSucceed, message, exceptions)
        => this.Value = value;

    public Result(TValue value, bool isSucceed, string? message = null, IEnumerable<Exception>? exceptions = null) : base(isSucceed, message, exceptions)
        => this.Value = value;

    public static Result<TValue?> Failed => _failed ??= new(default, false);
    public static Result<TValue?> Succeed => _succeed ??= new(default, true);
    public TValue Value { get; }

    public static implicit operator (Result Result, TValue? Value)(Result<TValue?> result)
        => (result, result.Value);

    public static implicit operator Result(Result<TValue?> result)
        => new(result.IsSucceed, result.Message, result.Exceptions);

    public static implicit operator TValue?(Result<TValue?> result)
        => result.Value;
}