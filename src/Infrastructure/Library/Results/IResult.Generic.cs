using Moshaveran.Library.Internals.Results;

using System.Diagnostics.CodeAnalysis;

namespace Moshaveran.Library.Results;

public interface IResult<out TValue> : IResult
{
    [NotNull]
    static new IResult<TValue?> Failed => Result<TValue?>.Failed;

    [NotNull]
    static new IResult<TValue?> Succeed => Result<TValue?>.Succeed;

    TValue Value { get; }
}