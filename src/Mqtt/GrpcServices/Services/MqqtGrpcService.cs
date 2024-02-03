using Grpc.Core;

using Moshaveran.API.Mqtt.GrpcServices.Protos;

namespace GrpcServices.Services;

public sealed class MqqtGrpcService(ILogger<MqqtGrpcService> logger) : MqqtReceiveSrvice.MqqtReceiveSrviceBase
{
    public override Task<PayloadReceivedResult> PayloadReceived(PayloadReceivedParams request, ServerCallContext context)
    {
        var result = new PayloadReceivedResult
        {
            Status = PayloadReceivedStatus.ReceivedSuccess,
        };
        if (request.SaveStatus == SaveStatus.SaveSuccess)
        {
            logger.LogInformation("[{Time}][{Imei}][{BrokerType}] : {log}", request.Time.ToDateTime(), request.IMEI, request.BrokerType, request.Log);
        }
        return Task.FromResult(result);
    }
}