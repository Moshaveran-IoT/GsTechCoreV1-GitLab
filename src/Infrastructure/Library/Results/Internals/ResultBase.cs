namespace Moshaveran.Library.Results.Internals;

public abstract class ResultBase(bool isSucceed, string? message = null, IEnumerable<Exception>? exceptions = null)
{
    public IEnumerable<Exception> Exceptions { get; } = exceptions?.Count() > 0 ? exceptions : [];

    public bool IsFailure => !this.IsSucceed;

    public bool IsSucceed { get; } = isSucceed;

    public string? Message { get; } = message;
}