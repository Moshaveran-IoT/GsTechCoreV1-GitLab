using GsTechCoreV1.Api.Controllers.MQTT.GsTech;

using Microsoft.Extensions.DependencyInjection;

using MQTTnet.AspNetCore;
using MQTTnet.AspNetCore.AttributeRouting;

namespace MqttServices;

public static class ServiceConfigurator
{
    public static IServiceCollection AddMqqt(this IServiceCollection services)
    {
        // Add Singleton MQTT Server object
        services.AddSingleton<GsTechMqttService>();

        // Add the MQTT Controllers
        services.AddMqttControllers();

        // Add the MQTT Service
        services
            .AddHostedMqttServerWithServices(aspNetMqttServerOptionsBuilder =>
            {
                var mqttService = aspNetMqttServerOptionsBuilder.ServiceProvider.GetRequiredService<GsTechMqttService>();
                mqttService.ConfigureMqttServerOptions(aspNetMqttServerOptionsBuilder);
                aspNetMqttServerOptionsBuilder.Build();
            })
            .AddMqttTcpServerAdapter()
            .AddMqttWebSocketServerAdapter()
            .AddMqttConnectionHandler()
            .AddConnections();

        // services.AddSingleton<MqttWebSocketServerAdapter>();
        // services.AddSingleton<IMqttServerAdapter>(s => s.GetService<MqttWebSocketServerAdapter>());
        services.AddScoped<MqttBaseController, MqttGsTechController>();
        return services;
    }
}
