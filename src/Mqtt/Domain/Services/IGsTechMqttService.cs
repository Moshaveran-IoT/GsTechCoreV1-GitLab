using Moshaveran.Library;
using Moshaveran.Library.ApplicationServices;

namespace Moshaveran.Mqtt.Domain.Services;

public interface IGsTechMqttService : IBusinessService
{
    Task<IResult> ProcessCameraPayload(ProcessPayloadArgs args);

    Task<IResult> ProcessCanPayload(ProcessPayloadArgs args);

    Task<IResult> ProcessGeneralPayload(ProcessPayloadArgs args);

    Task<IResult> ProcessGeneralPlusPayload(ProcessPayloadArgs args);

    Task<IResult> ProcessGpsPayload(ProcessPayloadArgs args);

    Task<IResult> ProcessObdPayload(ProcessPayloadArgs args);

    Task<IResult> ProcessSignalPayload(ProcessPayloadArgs args);

    Task<IResult> ProcessTemperaturePayload(ProcessPayloadArgs args);

    Task<IResult> ProcessTpmsPayload(ProcessPayloadArgs args);

    Task<IResult> ProcessVoltagePayload(ProcessPayloadArgs args);
}

public readonly record struct ProcessPayloadArgs(byte[] Payload, string ClientId, string Imei, CancellationToken Token = default);