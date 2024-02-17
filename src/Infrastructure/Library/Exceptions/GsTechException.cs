namespace Moshaveran.Library.Exceptions;

public sealed class GsTechException : GsTechExceptionBase, IGsTechException
{
    public GsTechException()
    {
    }

    public GsTechException(string? message) : base(message)
    {
    }

    public GsTechException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}