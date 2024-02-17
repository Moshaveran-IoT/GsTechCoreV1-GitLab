using Moshaveran.Library.Exceptions;

namespace Moshaveran.Library.Results.Internals;

public abstract class ResultBase(bool isSucceed, string? message = null, params IGsTechException[] exceptions)
{
    protected ResultBase(bool isSucceed, string? message = null, IEnumerable<IGsTechException>? exceptions = null)
        : this(isSucceed, message, exceptions?.ToArray() ?? [])
    {
    }

    public IEnumerable<IGsTechException> Exceptions { get; } = exceptions.Length != 0 ? exceptions : [];

    public bool IsFailure => !this.IsSucceed;

    public bool IsSucceed { get; } = isSucceed;

    public string? Message { get; } = message;
}