using Moshaveran.API.Mqtt.Application.Services;

using MQTTnet.AspNetCore.AttributeRouting;

namespace Moshaveran.API.Mqtt.API.Controllers;

[MqttController]
[MqttRoute("Gs")]
public class MqttGsTechController(ILogger<MqttGsTechController> logger, GsTechMqttService service) : MqttBaseController
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
    public void Image(string IMEI) => logger.LogInformation("*** Image Payload Received! IMEI: " + IMEI);

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
    public void TPMS(string IMEI) => logger.LogInformation("*** TPMS Payload Received! IMEI: " + IMEI);

    [MqttRoute("{IMEI}/Voltage")]
    public Task Voltage(string IMEI, CancellationToken token = default)
        => ProcessServiceMethod(service.ProcessVoltagePayload, "Voltage", IMEI, token);

    private Task ProcessServiceMethod(Func<byte[], string, CancellationToken, Task<Result>> method, string payloadName, string imei, CancellationToken token = default)
        => ProcessServiceMethod(() => method(Message.Payload, imei, token), payloadName, imei);

    private async Task ProcessServiceMethod(Func<Task<Result>> method, string payloadName, string imei)
    {
        logger.LogInformation($"*** {payloadName} Payload Received! IMEI: {imei}");
        var result = await method();
        if (result.IsSucceed)
        {
            await Ok();
        }
        else
        {
            await this.BadMessage();
        }
    }
}