namespace Moshaveran.Infrastructure.Mapping;

public interface IMappable<in TSource, out TDestination>
{
    TDestination? Map(TSource? self);
}
