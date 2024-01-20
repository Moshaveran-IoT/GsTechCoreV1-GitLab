using BackgroundServices;

using MQTTnet.AspNetCore;

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

                int appPort = int.Parse(config["AppPort"]);

                webBuilder.UseKestrel(o =>
                {
                    //TODO o.ListenAnyIP(1885, l => l.UseMqtt());
                    o.ListenAnyIP(appPort); // default http pipeline
                });

                webBuilder.UseStartup<Startup>();
            })
            .UseWindowsService();

    public static void Main(string[] args) =>
        CreateHostBuilder(args).Build().Run();
}