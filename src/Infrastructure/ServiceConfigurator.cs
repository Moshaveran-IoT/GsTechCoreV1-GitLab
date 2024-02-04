using Microsoft.Extensions.DependencyInjection;

using Moshaveran.Infrastructure.Mapping;

namespace Moshaveran.Infrastructure;

public static class ServiceConfigurator
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
        => services.AddScoped(_ => IMapper.New());
}