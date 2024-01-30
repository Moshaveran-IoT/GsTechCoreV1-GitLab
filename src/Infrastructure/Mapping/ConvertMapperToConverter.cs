using System.Diagnostics.CodeAnalysis;

using Moshaveran.Infrastructure.Interfaces;

namespace Moshaveran.Infrastructure.Mapping;

internal sealed class ConvertMapperToConverter<TSelf, TDestination, TMapper>(TSelf self, [DisallowNull] TMapper mapper) : IConvertible<TDestination?>
    where TMapper : IMappable<TSelf, TDestination>
{
    public TDestination? Convert()
        => mapper.Map(self);
}
