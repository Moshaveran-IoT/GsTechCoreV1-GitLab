using System.Diagnostics.CodeAnalysis;

namespace Moshaveran.Infrastructure.Mapping;

public interface IMapper
{
    static IMapper New()
        => new Mapper();

    IMapper ConfigureMapFor<TSource, TDestination>(Func<TSource, TDestination> mapper);

    IMapper ConfigureMapFor<TSource, TDestination>(Func<TDestination> customMapper);

    [return: NotNull]
    TDestination Map<TDestination>(in object source);

    TDestination Map<TDestination>(in object source, Func<TDestination> getDestination);

    TDestination Map<TSource, TDestination>(in TSource source, Func<TSource, TDestination> getDestination)
        => getDestination(source);

    TDestination Map<TDestination>(in object source, Func<object, TDestination> getDestination)
        => Map<object, TDestination>(source, getDestination);

    IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> items, Func<TSource, TDestination> getDestination)
        => items.Select(x => Map(x, getDestination));

    [return: NotNullIfNotNull(nameof(destination))]
    TDestination? Map<TDestination>(object source, in TDestination destination);
}