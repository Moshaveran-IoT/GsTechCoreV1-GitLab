using DataEntities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class DataAccessConfigurator
{
    public static IServiceCollection AddMqttDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddDbContext<MqttDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("ApplicationConnection"), 
                  b => b.MigrationsAssembly(typeof(DataAccessConfigurator).Assembly.FullName)), 
            ServiceLifetime.Transient);
        return services;
    }
}