using Grpc.Core;

using Moshaveran.API.Mqtt.GrpcServices.Protos;

namespace GrpcServices.Services;

public sealed class MqqtGrpcService(ILogger<MqqtGrpcService> logger) : MqqtReceiveSrvice.MqqtReceiveSrviceBase
{
    public override Task<CANReceivedResult> CANReceived(CANReceivedParams request, ServerCallContext context)
    {
        var result = new CANReceivedResult
        {
            Status = CANReceivedStatus.ReceivedSuccess,
        };
        if (request.SaveStatus == SaveStatus.SaveSuccess)
        {
            logger.LogInformation("*** CAN Payload Received! IMEI: {IMEI} at {Time}", request.IMEI, request.Time.ToDateTime());
        }
        return Task.FromResult(result);
    }
}