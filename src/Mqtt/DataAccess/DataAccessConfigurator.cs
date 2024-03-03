using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Moshaveran.GsTech.Mqtt.DataAccess.Repositories;
using Moshaveran.Library.Data;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

namespace Moshaveran.GsTech.Mqtt.DataAccess;

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
        var writeConnectionString = configuration.GetConnectionString("WriteDb");
        var readConnectionString = configuration.GetConnectionString("ReadDb");
        _ = services
            .AddDbContext<MqttWriteDbContext>(options => options.UseSqlServer(writeConnectionString, b =>
            {
                _ = b.MigrationsAssembly(typeof(DataAccessConfigurator).Assembly.FullName);
                _ = b.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            }), ServiceLifetime.Transient)
            .AddDbContext<MqttReadDbContext>(options =>
            {
                _ = options.UseSqlServer(readConnectionString, b =>
                {
                    _ = b.MigrationsAssembly(typeof(DataAccessConfigurator).Assembly.FullName);
                    _ = b.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
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
            .AddScoped<IRepository<TpmsBroker>>(x => new TpmsBrokerRepository(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()))
            .AddScoped<IRepository<CameraBroker>>(x => new CameraBrokerRepository(x.GetRequiredService<MqttReadDbContext>(), x.GetRequiredService<MqttWriteDbContext>()));

        return services;
    }
}