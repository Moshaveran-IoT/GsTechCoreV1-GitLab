using BackgroundServices;

internal class Program
{
    //private static void Main(string[] args)
    //{
    //    var builder = WebApplication.CreateBuilder(args);

    //    builder.Services.AddControllers();

    //    builder.Services.AddEndpointsApiExplorer();
    //    builder.Services.AddSwaggerGen();

    //    var app = builder.Build();

    //    // Configure the HTTP request pipeline.
    //    if (app.Environment.IsDevelopment())
    //    {
    //        app.UseSwagger();
    //        app.UseSwaggerUI();
    //    }

    //    app.UseHttpsRedirection();

    //    app.UseAuthorization();

    //    app.MapControllers();

    //    app.Run();
    //}

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
            .UseWindowsService();

    public static void Main(string[] args) =>
        CreateHostBuilder(args).Build().Run();
}