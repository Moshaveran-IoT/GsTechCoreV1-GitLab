using Moshaveran.Library.ApplicationServices;
using Moshaveran.Library.Results;

namespace Moshaveran.Mqtt.Domain.Services;

public interface IGsTechMqttService : IBusinessService
{
    Task<Result> ProcessCameraPayload(ProcessPayloadArgs args);

    Task<Result> ProcessCanPayload(ProcessPayloadArgs args);

    Task<Result> ProcessGeneralPayload(ProcessPayloadArgs args);

    Task<Result> ProcessGeneralPlusPayload(ProcessPayloadArgs args);

    Task<Result> ProcessGpsPayload(ProcessPayloadArgs args);

    Task<Result> ProcessObdPayload(ProcessPayloadArgs args);

    Task<Result> ProcessSignalPayload(ProcessPayloadArgs args);

    Task<Result> ProcessTemperaturePayload(ProcessPayloadArgs args);

    Task<Result> ProcessTpmsPayload(ProcessPayloadArgs args);

    Task<Result> ProcessVoltagePayload(ProcessPayloadArgs args);
}

public readonly record struct ProcessPayloadArgs(byte[] Payload, string ClientId, string Imei, CancellationToken Token = default);