using Moshaveran.Library.Results;

using System.Diagnostics;

namespace Moshaveran.Library.Internals.Results;

[DebuggerStepThrough, StackTraceHidden]
internal abstract class ResultBase(bool isSucceed, string? message = null, IEnumerable<Exception>? exceptions = null, IResult? innerResult = null)
{
    public IEnumerable<Exception> Exceptions { get; } = exceptions?.Count() > 0 ? exceptions : [];

    public IResult? InnerResult { get; } = innerResult;

    public bool IsFailure => !this.IsSucceed;

    public bool IsSucceed { get; } = isSucceed;

    public string? Message { get; } = message;
}