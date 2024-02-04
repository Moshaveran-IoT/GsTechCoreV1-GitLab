using Application.Interfaces;

using Google.Protobuf.WellKnownTypes;

using Grpc.Net.Client;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Moshaveran.API.Mqtt.GrpcServices.Protos;

namespace Application.Services;

public sealed class ListenerService : IListenerService
{
    private readonly MqqtReceiveService.MqqtReceiveServiceClient _grpcClient;
    private readonly ILogger<ListenerService> _logger;

    public ListenerService(IConfiguration configuration, ILogger<ListenerService> logger)
    {
        _grpcClient = createGrpcClient();

        MqqtReceiveService.MqqtReceiveServiceClient createGrpcClient()
        {
            var channel = GrpcChannel.ForAddress(configuration["GrpcServiceSettings:ServerUrl"]!);
            return new MqqtReceiveService.MqqtReceiveServiceClient(channel);
        }

        this._logger = logger;
    }

    public async Task LogClientConnectedAsync(string clientId, CancellationToken token = default)
    {
        try
        {
            _ = await _grpcClient.ClientConnectedAsync(new() { ClientId = clientId, Time = Now() }, cancellationToken: token);
        }
        catch
        {
            
        }
    }

    public async Task LogClientDisconnectedAsync(string clientId, CancellationToken token = default)
    {
        try
        {
            _ = await _grpcClient.ClientDisconnectedAsync(new() { ClientId = clientId, Time = Now() }, cancellationToken: token);
        }
        catch
        {

        }
    }

    public async Task LogPayloadReceivedAsync<TBroker>(LogPayloadReceivedArgs<TBroker> args, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(args);
        try
        {
            var grpcRequest = new PayloadReceivedParams
            {
                IMEI = args.Imei,
                Time = Now(),
                BrokerType = typeof(TBroker).Name,
                Log = args.LogMessage,
                SaveStatus = args.Status,
            };
            _ = await this._grpcClient.PayloadReceivedAsync(grpcRequest, cancellationToken: token);
        }
        catch
        {

        }
    }

    private static Timestamp Now() => Timestamp.FromDateTime(DateTime.UtcNow);
}