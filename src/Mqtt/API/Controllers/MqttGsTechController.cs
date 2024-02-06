using Application.Services;

using Moshaveran.Mqtt.Domain.Services;

using MQTTnet.AspNetCore.AttributeRouting;

namespace Moshaveran.API.Controllers;

[MqttController]
[MqttRoute("Gs")]
public class MqttGsTechController(GsTechMqttService service) : MqttBaseController
{
    [MqttRoute("{IMEI}/CAN")]
    public Task CAN(string IMEI, CancellationToken token = default)
        => ProcessServiceMethod(service.ProcessCanPayload, "CAN", IMEI, token);

    [MqttRoute("{IMEI}/General")]
    public Task General(string IMEI, CancellationToken token = default)
        => ProcessServiceMethod(service.ProcessGeneralPayload, "General", IMEI, token);

    [MqttRoute("{IMEI}/GeneralPlus")]
    public Task GeneralPlus(string IMEI, CancellationToken token = default)
        => ProcessServiceMethod(service.ProcessGeneralPlusPayload, "General Plus", IMEI, token);

    [MqttRoute("{IMEI}/GPS")]
    public Task GPS(string IMEI, CancellationToken token = default)
        => ProcessServiceMethod(service.ProcessGpsPayload, "GPS", IMEI, token);

    [MqttRoute("{IMEI}/Image")]
    public Task Image(string IMEI, CancellationToken token = default)
        => ProcessServiceMethod(service.ProcessCameraPayload, "Camera", IMEI, token);

    [MqttRoute("{IMEI}/OBD")]
    public Task OBD(string IMEI, CancellationToken token = default)
        => ProcessServiceMethod(service.ProcessObdPayload, "OBD", IMEI, token);

    [MqttRoute("{IMEI}/Signal")]
    public Task Signal(string IMEI, CancellationToken token = default)
        => ProcessServiceMethod(service.ProcessSignalPayload, "Signal", IMEI, token);

    [MqttRoute("{IMEI}/Temp")]
    public Task Temp(string IMEI, CancellationToken token = default)
        => ProcessServiceMethod(service.ProcessTemperaturePayload, "Temp", IMEI, token);

    [MqttRoute("{IMEI}/TPMS")]
    public Task TPMS(string IMEI, CancellationToken token = default)
        => ProcessServiceMethod(service.ProcessTpmsPayload, "TPMS", IMEI, token);

    [MqttRoute("{IMEI}/Voltage")]
    public Task Voltage(string IMEI, CancellationToken token = default)
        => ProcessServiceMethod(service.ProcessVoltagePayload, "Voltage", IMEI, token);

    private async Task ProcessServiceMethod(Func<ProcessPayloadArgs, Task<Result>> method, string _, string imei, CancellationToken token = default)
    {
        var result = await method(new(Message.Payload, this.MqttContext.ClientId, imei, token));
        if (result.IsSucceed)
        {
            await Ok();
        }
        else
        {
            await BadMessage();
        }
    }
}