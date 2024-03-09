namespace Moshaveran.GsTech.Mqtt.Domain.Dtos;

public sealed class ProcessTempPayloadDto(in byte[] payload, in string clientId, in string imei) : ProcessPayloadDto(payload, clientId, imei);
public sealed class ProcessTpmsPayloadDto(in byte[] payload, in string clientId, in string imei) : ProcessPayloadDto(payload, clientId, imei);