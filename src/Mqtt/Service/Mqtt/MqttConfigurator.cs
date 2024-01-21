using Moshaveran.Mqtt.DataAccess;

using Moshaveran.WinService.Mqtt.Controllers;
using Moshaveran.WinService.Mqtt.Services;

using MQTTnet.AspNetCore;
using MQTTnet.AspNetCore.AttributeRouting;
using MQTTnet.AspNetCore.Extensions;

namespace Moshaveran.WinService.Mqtt;

public static class MqttConfigurator
{
    public static IServiceCollection AddMqttServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<GsTechMqttService>();
        _ = services.AddAutoMapper(typeof(Startup));
        _ = services.AddMqttNetServices();
        _ = services.AddMqttDataAccessServices(configuration);
        return services;
    }

    public static IApplicationBuilder ConfigureMqtt(this IApplicationBuilder app, int portNo)
        => app.UseEndpoints(endpoints =>
        {
            _ = endpoints.MapConnectionHandler<MqttConnectionHandler>("/mqtt", e => e.WebSockets.SubProtocolSelector = p => p.FirstOrDefault() ?? string.Empty);
        }).UseMqttServer(server =>
        {
            app.ApplicationServices.GetRequiredService<GsTechMqttInterceptorService>().ConfigureMqttServer(server);
        });

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
                // Enable Attribute Routing
                // By default, messages published to topics that don't match any routes are rejected.
                // Change this to true to allow those messages to be routed without hitting any controller actions.
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