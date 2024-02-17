using Moshaveran.Library.Exceptions;

namespace Moshaveran.Library.Results;

public interface IResult
{
    IEnumerable<IGsTechException> Exceptions { get; }
    bool IsFailure { get; }
    bool IsSucceed { get; }
    string? Message { get; }
}