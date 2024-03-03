using System.Diagnostics;

namespace Moshaveran.Library.Internals.Results;

[DebuggerStepThrough, StackTraceHidden]
internal abstract class ResultBase(bool isSucceed, string? message = null, IEnumerable<Exception>? exceptions = null)
{
    public IEnumerable<Exception> Exceptions { get; } = exceptions?.Count() > 0 ? exceptions : [];

    public bool IsFailure => !IsSucceed;

    public bool IsSucceed { get; } = isSucceed;

    public string? Message { get; } = message;
}