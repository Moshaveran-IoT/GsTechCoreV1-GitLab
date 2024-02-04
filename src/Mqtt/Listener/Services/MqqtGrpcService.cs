using Google.Protobuf.WellKnownTypes;

using Grpc.Core;

using Moshaveran.API.Mqtt.GrpcServices.Protos;

namespace Moshaveran.Mqtt.Listener.Services;

public sealed class MqqtGrpcService(ILogger<MqqtGrpcService> logger) : MqqtReceiveService.MqqtReceiveServiceBase
{
    public override Task<Empty> ClientConnected(ClientConnectedArgs request, ServerCallContext context)
    {
        logger.LogInformation("[{Time}][{ClientId}] : Connected", request.Time.ToDateTime(), request.ClientId);
        return Task.FromResult(new Empty());
    }

    public override Task<Empty> ClientDisconnected(ClientDisconnectedArgs request, ServerCallContext context)
    {
        logger.LogInformation("[{Time}][{ClientId}] : Disconnected", request.Time.ToDateTime(), request.ClientId);
        return Task.FromResult(new Empty());
    }
    
    public override Task<PayloadReceivedResult> PayloadReceived(PayloadReceivedParams request, ServerCallContext context)
    {
        var result = new PayloadReceivedResult
        {
            Status = PayloadReceivedStatus.ReceivedSuccess,
        };
        logger.LogInformation("[{Time}][{Imei}][{BrokerType}] : {log}", request.Time.ToDateTime(), request.IMEI, request.BrokerType, request.Log);
        return Task.FromResult(result);
    }
}