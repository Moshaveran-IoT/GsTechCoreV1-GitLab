namespace Moshaveran.Library.Exceptions;

public sealed class InvalidOperationGsTechException : GsTechExceptionBase, IGsTechException
{
    public InvalidOperationGsTechException()
    {
    }

    public InvalidOperationGsTechException(string? message) : base(message)
    {
    }

    public InvalidOperationGsTechException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}