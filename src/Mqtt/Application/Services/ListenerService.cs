using Application.Interfaces;

using Google.Protobuf.WellKnownTypes;

using Grpc.Net.Client;

using Microsoft.Extensions.Configuration;

using Moshaveran.API.Mqtt.GrpcServices.Protos;

namespace Application.Services;

public sealed class ListenerService : IListenerService
{
    private readonly MqqtReceiveSrvice.MqqtReceiveSrviceClient _grpcClient;

    public ListenerService(IConfiguration configuration)
    {
        _grpcClient = createGrpcClient();

        MqqtReceiveSrvice.MqqtReceiveSrviceClient createGrpcClient()
        {
            var channel = GrpcChannel.ForAddress(configuration["GrpcServiceSettings:ServerUrl"]!);
            return new MqqtReceiveSrvice.MqqtReceiveSrviceClient(channel);
        }
    }

    public async Task LogPayloadReceivedAsync<TBroker>(LogPayloadReceivedArgs<TBroker> args)
    {
        ArgumentNullException.ThrowIfNull(args);
        try
        {
            var grpcRequest = new PayloadReceivedParams
            {
                IMEI = args.Imei,
                Time = Timestamp.FromDateTime(DateTime.UtcNow),
                BrokerType = typeof(TBroker).Name,
                Log = args.LogMessage,
                SaveStatus = args.Status,
            };
            _ = await this._grpcClient.PayloadReceivedAsync(grpcRequest);
        }
        catch
        {
        }
    }
}