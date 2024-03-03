using Moshaveran.Library.Internals.Results;

namespace Moshaveran.Library.Results;

public interface IResult<TValue> : IResult
{
    static new IResult<TValue?> Failed => Result<TValue?>.Failed;
    static new IResult<TValue?> Succeed => Result<TValue?>.Succeed;

    TValue Value { get; }
}