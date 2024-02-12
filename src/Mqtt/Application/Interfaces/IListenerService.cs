using System.Diagnostics.Contracts;

using Moshaveran.API.Mqtt.GrpcServices.Protos;
using Moshaveran.Library.ApplicationServices;

namespace Moshaveran.GsTech.Mqtt.Application.Interfaces;

public interface IListenerService : IBusinessService
{
    [Pure]
    Task LogClientConnectedAsync(string clientId, CancellationToken token = default);

    [Pure]
    Task LogClientDisconnectedAsync(string clientId, CancellationToken token = default);

    [Pure]
    Task LogPayloadReceivedAsync<TBroker>(LogPayloadReceivedArgs<TBroker> args, CancellationToken token = default);
}

public sealed record LogPayloadReceivedArgs<TBroker>(string ClientId, string Imei, string LogMessage, SaveStatus Status);