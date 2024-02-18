namespace Moshaveran.Library;

public interface IResult
{
    IEnumerable<Exception> Exceptions { get; }
    bool IsFailure { get; }
    bool IsSucceed { get; }
    string? Message { get; }
}