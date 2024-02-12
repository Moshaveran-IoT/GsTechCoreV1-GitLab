using Microsoft.Extensions.DependencyInjection;
using Moshaveran.Library.Mapping;

namespace Moshaveran.Library;

public static class ServiceConfigurator
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
        => services.AddScoped(_ => IMapper.New());
}