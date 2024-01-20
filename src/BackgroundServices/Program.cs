using Moshaveran.BackgroundServices;
using Moshaveran.BackgroundServices.MqttServices;

using MQTTnet.AspNetCore.Extensions;

internal class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                var appPort = int.Parse(config["AppPort"]);
                _ = webBuilder.UseKestrel(o =>
                {
                    o.ListenAnyIP(1885, l => l.UseMqtt());
                    o.ListenAnyIP(appPort); // default http pipeline
                });

                _ = webBuilder.UseStartup<Startup>();
            })
            .UseWindowsService();

    public static void Main(string[] args) =>
        CreateHostBuilder(args).Build().Run();
}