using System.Diagnostics.CodeAnalysis;
using Moshaveran.Library.Interfaces;

namespace Moshaveran.Library.Mapping;

internal sealed class ConvertMapperToConverter<TSelf, TDestination, TMapper>(TSelf self, [DisallowNull] TMapper mapper) : IConvertible<TDestination?>
    where TMapper : IMappable<TSelf, TDestination>
{
    public TDestination? Convert()
        => mapper.Map(self);
}
