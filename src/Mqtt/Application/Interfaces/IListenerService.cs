using System.Diagnostics.Contracts;

using Moshaveran.API.Mqtt.GrpcServices.Protos;
using Moshaveran.Infrastructure.ApplicationServices;

namespace Application.Interfaces;

public interface IListenerService : IBusinessService
{
    [Pure]
    Task LogPayloadReceivedAsync<TBroker>(LogPayloadReceivedArgs<TBroker> args);
}

public sealed record LogPayloadReceivedArgs<TBroker>(string Imei, string LogMessage, SaveStatus Status);