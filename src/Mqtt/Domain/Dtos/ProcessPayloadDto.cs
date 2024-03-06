namespace Moshaveran.GsTech.Mqtt.Domain.Dtos;

public abstract class ProcessPayloadDto(in byte[] payload, in string clientId, in string imei)
{
    public string ClientId { get; } = clientId;
    public string Imei { get; } = imei;
    public byte[] Payload { get; } = payload;
}
