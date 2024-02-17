using Moshaveran.Library.Exceptions;

namespace Moshaveran.Library.Results.Internals;

public abstract class ResultBase(bool isSucceed, string? message = null, params Exception[] exceptions)
{
    protected ResultBase(bool isSucceed, string? message = null, IEnumerable<Exception>? exceptions = null)
        : this(isSucceed, message, exceptions?.ToArray() ?? [])
    {
    }

    public IEnumerable<Exception> Exceptions { get; } = exceptions.Length != 0 ? exceptions : [];

    public bool IsFailure => !this.IsSucceed;

    public bool IsSucceed { get; } = isSucceed;

    public string? Message { get; } = message;
}