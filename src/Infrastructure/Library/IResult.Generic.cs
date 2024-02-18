namespace Moshaveran.Library;

public interface IResult<TValue> : IResult
{
    TValue Value { get; }
}