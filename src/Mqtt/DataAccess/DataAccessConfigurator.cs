using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;
using Moshaveran.Mqtt.DataAccess.Repositories;
using Moshaveran.Mqtt.DataAccess.Repositories.Bases;

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
        // Add DbContexts
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

        // Add repositories
        _ = services
            .AddScoped<IRepository<CanBroker>>(x => new CanBrokerRepository(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()))
            .AddScoped<IRepository<GeneralBroker>>(x => new GeneralBrokerRepository(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()))
            //todo: GeneralPlusBroker strategy pattern must be re-written.
            //x .AddScoped<IRepository<GeneralPlusBroker>>(x => new GeneralPlusBrokerRepository(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()))
            .AddScoped<IRepository<SignalBroker>>(x => new SignalBrokerRepository(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()))
            .AddScoped<IRepository<VoltageBroker>>(x => new VoltageBrokerRepository(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()))
            .AddScoped<IRepository<GpsBroker>>(x => new GpsBrokerRepository(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()))
            .AddScoped<IRepository<ObdBroker>>(x => new ObdBrokerRepository(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()))
            .AddScoped<IRepository<TemperatureBroker>>(x => new TemperatureBrokerRepository(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()))
            .AddScoped<IRepository<TpmsBroker>>(x => new TpmsBrokerRepository(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()));

        return services;
    }
}