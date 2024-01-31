using Moshaveran.API.Mqtt;
using Moshaveran.API.Mqtt.Application.Services;
using Moshaveran.Infrastructure;

using Prometheus;

namespace Moshaveran.API;

public class Startup(IConfiguration configuration)
{
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Add Prometheus metrics service
        app.UseHttpMetrics();
        //app.MapMetrics();

        if (env.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MQTT API V3");
            });
        }

        _ = app.UseRouting();

        _ = app.UseEndpoints(endpoints =>
        {
            _ = endpoints.MapControllers();
            _ = endpoints.MapGet("Hi", () => "Hello from Mohammad");
        });

        _ = app.ConfigureMqtt(1545);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // // Add Prometheus metrics service
        services.AddMetricServer(options => { options.Port = 5678; });

        // Add project services
        _ = services.AddInfrastructureService()
            .AddMqttServices(configuration);

        // Setup api
        _ = services.AddControllers();
        _ = services.AddSwaggerGen();
    }
}