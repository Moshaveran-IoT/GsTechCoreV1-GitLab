namespace Moshaveran.Library.Results;

public interface IResult<TValue>
{
    TValue Value { get; }
}