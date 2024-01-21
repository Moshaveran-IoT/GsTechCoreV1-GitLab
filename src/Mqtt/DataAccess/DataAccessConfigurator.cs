using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;
using Moshaveran.Mqtt.DataAccess.Repositories;

namespace Moshaveran.Mqtt.DataAccess;

public static class DataAccessConfigurator
{
    public static IServiceCollection AddMqttDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ApplicationConnection");
        _ = services.AddDbContext<MqttReadDbContext>(
            options =>
            {
                _ = options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(DataAccessConfigurator).Assembly.FullName));
                _ = options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            },
            ServiceLifetime.Transient);
        _ = services.AddDbContext<MqttWriteDbContext>(
            options => options.UseSqlServer(connectionString,
                  b => b.MigrationsAssembly(typeof(DataAccessConfigurator).Assembly.FullName)),
            ServiceLifetime.Transient);
        _ = services.AddTransient<IRepository<CanBroker>>(x => new GenericRepository<CanBroker>(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()));
        return services;
    }
}