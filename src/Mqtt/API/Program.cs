using MQTTnet.AspNetCore.Extensions;

namespace Moshaveran.GsTech.Mqtt.API;

internal static class Program
{
    public static void Main(string[] args)
    {
        string? environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        Console.Title = "GSTech IoT Service";
        createHostBuilder(args).Build().Run();
        static IHostBuilder createHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       var config = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                           .Build();

                       var appPort = int.Parse(config["AppPort"]!);
                       _ = webBuilder.UseKestrel(o =>
                       {
                           o.ListenAnyIP(1885, l => l.UseMqtt());
                           o.ListenAnyIP(appPort); // default http pipeline
                       });

                       _ = webBuilder.UseStartup<Startup>();
                   })
            //.UseWindowsService()
            ;
    }
}