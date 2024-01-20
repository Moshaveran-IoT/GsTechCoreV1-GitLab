using Microsoft.Extensions.DependencyInjection;

namespace Moshaveran.Infrastructure;

public static class ServiceConfigurator
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services)
    {
        return services.AddScoped(_ => IMapper.New());
    }
}