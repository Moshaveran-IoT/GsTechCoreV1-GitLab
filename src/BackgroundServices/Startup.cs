using Moshaveran.BackgroundServices.MqttServices;

namespace Moshaveran.BackgroundServices;

public class Startup
{
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        //if (env.IsDevelopment())
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
        _ = services.AddControllers();
        _ = services.AddMqttServices();
        _ = services.AddSwaggerGen();
    }
}