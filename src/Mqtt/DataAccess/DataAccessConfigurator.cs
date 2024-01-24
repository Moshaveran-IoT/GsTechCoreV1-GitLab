using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;
using Moshaveran.Mqtt.DataAccess.Repositories;

namespace Moshaveran.Mqtt.DataAccess;

public static class DataAccessConfigurator
{
    /// <summary>
    /// Adds the MQTT module's data access services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static IServiceCollection AddMqttDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ApplicationConnection");
        _ = services
            .AddDbContext<MqttWriteDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(DataAccessConfigurator).Assembly.FullName)), ServiceLifetime.Transient)
            .AddDbContext<MqttReadDbContext>(
                options =>
                {
                    _ = options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(DataAccessConfigurator).Assembly.FullName));
                    _ = options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                }, ServiceLifetime.Transient)
            ;

        _ = services.AddScoped<IRepository<CanBroker>>(x => new GenericRepository<CanBroker>(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()))
            .AddScoped<IRepository<GeneralBroker>>(x => new GenericRepository<GeneralBroker>(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()))
            .AddScoped<IRepository<SignalBroker>>(x => new GenericRepository<SignalBroker>(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()))
            .AddScoped<IRepository<VoltageBroker>>(x => new GenericRepository<VoltageBroker>(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()))
            .AddScoped<IRepository<GpsBroker>>(x => new GenericRepository<GpsBroker>(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()))
            .AddScoped<IRepository<ObdBroker>>(x => new GenericRepository<ObdBroker>(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()));

        return services;
    }
}