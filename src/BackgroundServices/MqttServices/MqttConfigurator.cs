using Moshaveran.BackgroundServices.MqttServices.Controllers;
using Moshaveran.BackgroundServices.MqttServices.Services;

using MQTTnet.AspNetCore;
using MQTTnet.AspNetCore.AttributeRouting;
using MQTTnet.AspNetCore.Extensions;

namespace Moshaveran.BackgroundServices.MqttServices;

public static class MqttConfigurator
{
    public static IServiceCollection AddMqttServices(this IServiceCollection services)
    {
        // Add Singleton MQTT Server object
        _ = services.AddSingleton<GsTechMqttService>();

        // Add the MQTT Controllers
        _ = services.AddMqttControllers();

        // Add the MQTT Service
        _ = services
            .AddHostedMqttServerWithServices(aspNetMqttServerOptionsBuilder =>
            {
                var mqttService = aspNetMqttServerOptionsBuilder.ServiceProvider.GetRequiredService<GsTechMqttService>();
                mqttService.ConfigureMqttServerOptions(aspNetMqttServerOptionsBuilder);
                _ = aspNetMqttServerOptionsBuilder.Build();
            })
            .AddMqttTcpServerAdapter()
            .AddMqttWebSocketServerAdapter()
            .AddMqttConnectionHandler()
            .AddConnections();

        _ = services.AddScoped<MqttBaseController, MqttGsTechController>();

        return services;
    }

    public static IApplicationBuilder ConfigureMqtt(this IApplicationBuilder app)
    {
        
        return app.UseEndpoints(endpoints =>
        {
            _ = endpoints.MapConnectionHandler<MqttConnectionHandler>("/mqtt", e => e.WebSockets.SubProtocolSelector = p => p.FirstOrDefault() ?? string.Empty);
        }).UseMqttServer(server =>
        {
            app.ApplicationServices.GetRequiredService<GsTechMqttService>().ConfigureMqttServer(server);
        });
    }
}