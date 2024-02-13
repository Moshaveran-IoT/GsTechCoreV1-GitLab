namespace Moshaveran.Library.Exceptions;

[Serializable]
public abstract class GsTechExceptionBase : Exception
{
    protected GsTechExceptionBase()
    {
    }

    protected GsTechExceptionBase(string? message) : base(message)
    {
    }

    protected GsTechExceptionBase(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}