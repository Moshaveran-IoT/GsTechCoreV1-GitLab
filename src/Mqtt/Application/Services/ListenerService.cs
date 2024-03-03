using Google.Protobuf.WellKnownTypes;

using Grpc.Net.Client;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Moshaveran.API.Mqtt.GrpcServices.Protos;
using Moshaveran.GsTech.Mqtt.Application.Interfaces;

namespace Moshaveran.GsTech.Mqtt.Application.Services;

public sealed class ListenerService : IListenerService
{
    private readonly MqqtReceiveService.MqqtReceiveServiceClient _grpcClient;
    private readonly ILogger<ListenerService> _logger;

    public ListenerService(IConfiguration configuration, ILogger<ListenerService> logger, ILoggerFactory loggerFactory)
    {
        this._logger = logger;
        this._grpcClient = createGrpcClient();

        MqqtReceiveService.MqqtReceiveServiceClient createGrpcClient()
        {
            var opt = new GrpcChannelOptions
            {
                LoggerFactory = loggerFactory,
            };
            var channel = GrpcChannel.ForAddress(configuration["GrpcServiceSettings:ServerUrl"]!, opt);
            return new MqqtReceiveService.MqqtReceiveServiceClient(channel);
        }
    }

    public async Task LogClientConnectedAsync(string clientId, CancellationToken token = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(clientId);
        try
        {
            _ = await this._grpcClient.ClientConnectedAsync(new()
            {
                ClientId = clientId,
                Time = Now()
            }, cancellationToken: token);
        }
        catch
        {
            this._logger.LogInformation("Client connected: {ClientId}", clientId);
        }
    }

    public async Task LogClientDisconnectedAsync(string clientId, CancellationToken token = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(clientId);
        try
        {
            _ = await this._grpcClient.ClientDisconnectedAsync(new()
            {
                ClientId = clientId,
                Time = Now()
            }, cancellationToken: token);
        }
        catch
        {
            this._logger.LogInformation("Client disconnected: {ClientId}", clientId);
        }
    }

    public async Task LogPayloadReceivedAsync<TBroker>(LogPayloadReceivedArgs<TBroker> args, CancellationToken token = default)
    {
        ArgumentNullException.ThrowIfNull(args);
        try
        {
            var grpcRequest = new PayloadReceivedParams
            {
                ClientID = args.ClientId,
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
            this._logger.LogInformation("[ID: {ClientId}][IMES: {IMEI}]: {LogMessage}", args.ClientId, args.Imei, args.LogMessage);
        }
    }

    private static Timestamp Now() => Timestamp.FromDateTime(DateTime.UtcNow);
}