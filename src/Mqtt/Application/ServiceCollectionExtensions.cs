using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace Moshaveran.GsTech.Mqtt.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining(typeof(ServiceCollectionExtensions)));
        return services;
    }
}