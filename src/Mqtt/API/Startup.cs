using Moshaveran.GsTech.Mqtt.API.Middlewares;
using Moshaveran.GsTech.Mqtt.Application;
using Moshaveran.GsTech.Mqtt.Domain;

using Prometheus;

namespace Moshaveran.GsTech.Mqtt.API;

public class Startup(IConfiguration configuration)
{
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    {
        _ = app.UseExceptionHandler();
        _ = app.UseLoggerMiddleware();

        // Add Prometheus metrics service
        _ = app.UseHttpMetrics();
        //app.MapMetrics();

        if (env.IsDevelopment())
        {
            _ = app.UseDeveloperExceptionPage();
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MQTT API V3"));
        }

        _ = app.UseRouting();

        _ = app.UseEndpoints(endpoints =>
        {
            _ = endpoints.MapControllers();
            _ = endpoints.MapGet("Hi", () => "Hello from Mohammad");
        });

        _ = app.ConfigureMqtt(int.Parse(configuration["AppPort"]!));
    }

    public void ConfigureServices(IServiceCollection services)
    {
        _ = services.AddExceptionHandler<GlobalExceptionHander>().AddProblemDetails();

        // Add Prometheus metrics service
        if (configuration.GetValue<bool?>("Prometheus:metrics:is_enabled") is true)
        {
            _ = services.AddMetricServer(options => options.Port = configuration.GetValue<ushort>("Prometheus:metrics:port"));
        }

        _ = services
            .AddInfrastructureService()
            .AddApplicationLayer(configuration)
            .AddMqttServices(configuration);


        // Setup api
        _ = services.AddControllers();
        _ = services.AddSwaggerGen();
    }
}