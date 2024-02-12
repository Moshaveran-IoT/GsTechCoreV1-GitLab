using Moshaveran.Mqtt.Monitor.Services;

var builder = WebApplication.CreateBuilder(args);
_ = builder.Services.AddLogging(o => o.AddConsole())
    .AddGrpc(o => o.EnableDetailedErrors = true);

var app = builder.Build();

app.UseRouting();

app.MapGrpcService<MqqtGrpcService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

Console.Title = "gRPC Monitoring Service";
app.Run();