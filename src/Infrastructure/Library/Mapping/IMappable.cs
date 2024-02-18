namespace Moshaveran.Library.Mapping;

public interface IMappable<in TSource, out TDestination>
{
    TDestination? Map(TSource? self);
}
