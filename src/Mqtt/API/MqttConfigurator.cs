using Application.Services;

using Moshaveran.GsTech.Mqtt.API.Controllers;
using Moshaveran.GsTech.Mqtt.Application.Interfaces;
using Moshaveran.GsTech.Mqtt.Application.Services;
using Moshaveran.GsTech.Mqtt.DataAccess;
using Moshaveran.Mqtt.Domain.Services;

using MQTTnet.AspNetCore;
using MQTTnet.AspNetCore.AttributeRouting;
using MQTTnet.AspNetCore.Extensions;

namespace Moshaveran.GsTech.Mqtt.API;

public static class MqttConfigurator
{
    /// <summary>
    /// Adds MQTT services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static IServiceCollection AddMqttServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddScoped<IGeocodingService>(_ => new GeocodingService());
        _ = services.AddSingleton<IListenerService, ListenerService>();
        _ = services.AddScoped<GsTechMqttService>();
        _ = services.AddMqttNetServices();
        _ = services.AddMqttDataAccessServices(configuration);
        return services;
    }

    /// <summary>
    /// Configures MQTT.
    /// </summary>
    /// <param name="app">The application.</param>
    /// <param name="portNo">The port no.</param>
    public static IApplicationBuilder ConfigureMqtt(this IApplicationBuilder app, int portNo) =>
        app.UseEndpoints(endpoints => _ = endpoints.MapConnectionHandler<MqttConnectionHandler>("/mqtt", e => e.WebSockets.SubProtocolSelector = p => p.FirstOrDefault() ?? string.Empty))
        .UseMqttServer(server => app.ApplicationServices.GetRequiredService<GsTechMqttInterceptorService>().ConfigureMqttServer(server));

    private static IServiceCollection AddMqttNetServices(this IServiceCollection services)
    {
        // Add Singleton MQTT Server object
        _ = services.AddSingleton<GsTechMqttInterceptorService>();

        // Add the MQTT Controllers
        _ = services.AddMqttControllers();

        // Add the MQTT Service
        _ = services
            .AddHostedMqttServerWithServices(options =>
            {
                var mqttService = options.ServiceProvider.GetRequiredService<GsTechMqttInterceptorService>();
                // Configure the MQTT Server options here
                _ = options.WithoutDefaultEndpoint()
                           .WithDefaultEndpointPort(1885)
                           .WithConnectionValidator(mqttService)
                           .WithSubscriptionInterceptor(mqttService);
                // Enable Attribute Routing By default, messages published to topics that don't
                // match any routes are rejected. Change this to true to allow those messages to be
                // routed without hitting any controller actions.
                _ = options.WithAttributeRouting(true);

                _ = options.Build();
            })
            .AddMqttTcpServerAdapter()
            .AddMqttWebSocketServerAdapter()
            .AddMqttConnectionHandler()
            .AddConnections();

        _ = services.AddScoped<MqttBaseController, MqttGsTechController>();
        return services;
    }
}