using Moshaveran.Mqtt.API.Middlewares;

using Prometheus;

namespace Moshaveran.Mqtt.API;

public class Startup(IConfiguration configuration)
{
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration)
    {
        app.UseExceptionHandler();

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
        services.AddExceptionHandler<GlobalExceptionHander>()
                .AddProblemDetails();

        // // Add Prometheus metrics service
        _ = services.AddMetricServer(options => options.Port = 5678);

        // Add project services
        _ = services.AddInfrastructureService()
            .AddMqttServices(configuration);

        // Setup api
        _ = services.AddControllers();
        _ = services.AddSwaggerGen();
    }
}