using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Moshaveran.GsTech.Mqtt.Application;

public static class ApplicationModule
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining(typeof(ApplicationModule)));
        return services;
    }
}