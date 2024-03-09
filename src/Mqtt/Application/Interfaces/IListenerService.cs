using Moshaveran.API.Mqtt.GrpcServices.Protos;
using Moshaveran.Library.ApplicationServices;

namespace Moshaveran.GsTech.Mqtt.Application.Interfaces;

public interface IListenerService : IBusinessService
{
    Task LogClientConnectedAsync(string clientId, CancellationToken token = default);

    Task LogClientDisconnectedAsync(string clientId, CancellationToken token = default);

    Task LogPayloadReceivedAsync<TBroker>(LogPayloadReceivedArgs<TBroker> args, CancellationToken token = default);
}

public sealed record LogPayloadReceivedArgs<TBroker>(string ClientId, string Imei, string LogMessage, SaveStatus Status);