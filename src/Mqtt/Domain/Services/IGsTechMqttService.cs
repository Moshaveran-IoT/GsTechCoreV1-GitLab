using Moshaveran.Infrastructure.ApplicationServices;
using Moshaveran.Infrastructure.Results;

namespace Moshaveran.Mqtt.Domain.Services;

public interface IGsTechMqttService : IBusinessService
{
    Task<Result> ProcessCameraPayload(byte[] payload, string imei, CancellationToken token = default);

    Task<Result> ProcessCanPayload(byte[] payload, string imei, CancellationToken token = default);

    Task<Result> ProcessGeneralPayload(byte[] payload, string imei, CancellationToken token = default);

    Task<Result> ProcessGeneralPlusPayload(byte[] payload, string imei, CancellationToken token = default);

    Task<Result> ProcessGpsPayload(byte[] payload, string imei, CancellationToken token = default);

    Task<Result> ProcessObdPayload(byte[] payload, string imei, CancellationToken token = default);

    Task<Result> ProcessSignalPayload(byte[] payload, string imei, CancellationToken token = default);

    Task<Result> ProcessTemperaturePayload(byte[] payload, string imei, CancellationToken token = default);

    Task<Result> ProcessTpmsPayload(byte[] payload, string imei, CancellationToken token = default);

    Task<Result> ProcessVoltagePayload(byte[] payload, string imei, CancellationToken token = default);
}