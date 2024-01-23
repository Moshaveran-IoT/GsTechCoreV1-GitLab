using Moshaveran.API.Mqtt.Application.Services;
using Moshaveran.Infrastructure.Results;

using MQTTnet.AspNetCore.AttributeRouting;

namespace Moshaveran.API.Mqtt.API.Controllers;

[MqttController]
[MqttRoute("Gs")]
public class MqttGsTechController(ILogger<MqttGsTechController> logger, GsTechMqttService service) : MqttBaseController
{
    [MqttRoute("{IMEI}/CAN")]
    public async Task CAN(string IMEI, CancellationToken token = default)
        => await ProcessServiceMethod(service.ProcessCanPayload, "CAN", IMEI, token);

    [MqttRoute("{IMEI}/General")]
    public async Task General(string IMEI, CancellationToken token = default)
        => await ProcessServiceMethod(service.ProcessGeneralPayload, "General", IMEI, token);

    [MqttRoute("{IMEI}/GeneralPlus")]
    public async Task GeneralPlus(string IMEI, CancellationToken token = default)
        => await ProcessServiceMethod(service.ProcessGeneralPlusPayload, "General Plus", IMEI, token);

    [MqttRoute("{IMEI}/GPS")]
    public  async Task GPS(string IMEI, CancellationToken token = default)
        => await ProcessServiceMethod(service.ProcessGpsPayload, "GPS", IMEI, token);

    [MqttRoute("{IMEI}/Image")]
    public void Image(string IMEI) => logger.LogInformation("*** Image Payload Received! IMEI: " + IMEI);

    [MqttRoute("{IMEI}/OBD")]
    public void OBD(string IMEI) => logger.LogInformation("*** OBD Payload Received! IMEI: " + IMEI);

    [MqttRoute("{IMEI}/Signal")]
    public async Task Signal(string IMEI, CancellationToken token = default)
        => await ProcessServiceMethod(service.ProcessSignalPayload, "Signal", IMEI, token);

    [MqttRoute("{IMEI}/Temp")]
    public void Temp(string IMEI) => logger.LogInformation("*** Temperature Payload Received! IMEI: " + IMEI);

    [MqttRoute("{IMEI}/TPMS")]
    public void TPMS(string IMEI) => logger.LogInformation("*** TPMS Payload Received! IMEI: " + IMEI);

    [MqttRoute("{IMEI}/Voltage")]
    public async Task Voltage(string IMEI, CancellationToken token = default)
        => await ProcessServiceMethod(service.ProcessVoltagePayload, "Voltage", IMEI, token);

    private async Task ProcessServiceMethod(Func<byte[], string, CancellationToken, Task<Result>> method, string payloadName, string imei, CancellationToken token = default)
        => await ProcessServiceMethod(() => method(Message.Payload, imei, token), payloadName, imei);

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