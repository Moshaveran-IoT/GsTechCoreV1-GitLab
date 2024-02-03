using GrpcServices.Services;

var builder = WebApplication.CreateBuilder(args);
_ = builder.Services.AddLogging(o => o.AddConsole());
_ = builder.Services.AddGrpc(o =>
{
    o.EnableDetailedErrors = true;
    //o.MaxReceiveMessageSize = 1024;
    //o.MaxSendMessageSize = 1024;
});

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapGrpcService<MqqtGrpcService>();
});

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();