namespace Moshaveran.Library.Exceptions;

public sealed class NotSupportedGsTechException : GsTechExceptionBase
{
    public NotSupportedGsTechException()
    {
    }

    public NotSupportedGsTechException(string? message) : base(message)
    {
    }

    public NotSupportedGsTechException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
