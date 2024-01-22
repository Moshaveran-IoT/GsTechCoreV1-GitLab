using System.Text;

using Moshaveran.API.Mqtt.Application.Services;

using MQTTnet.AspNetCore.AttributeRouting;

namespace Moshaveran.API.Mqtt.API.Controllers;

[MqttController]
public class CatchAllController(ILogger<CatchAllController> logger) : MqttBaseController
{
    [MqttRoute("{*topic}")]
    public Task WildCardMatchTopic(string topic)
    {
        var payloadMessage = Encoding.UTF8.GetString(Message.Payload);
        logger.LogInformation($"Wildcard matched on Topic: '{topic}'");
        logger.LogInformation($"{payloadMessage}");
        return Ok();
    }
}

[MqttController]
[MqttRoute("Gs")]
public class MqttGsTechController(ILogger<MqttGsTechController> logger, GsTechMqttService service) : MqttBaseController
{
    [MqttRoute("{IMEI}/CAN")]
    public async Task CAN(string IMEI)
    {
        logger.LogInformation("*** CAN Payload Received! IMEI: " + IMEI);
        var result = await service.InsertCanBroker(Message.Payload, IMEI);
        if (result.IsSucceed)
            await Ok();
        else
            await this.BadMessage();
    }

    [MqttRoute("{IMEI}/General")]
    public void General(string IMEI) => logger.LogInformation("*** General Payload Received! IMEI: " + IMEI);

    [MqttRoute("{IMEI}/GeneralPlus")]
    public void GeneralPlus(string IMEI) => logger.LogInformation("*** General Payload Received! IMEI: " + IMEI);

    [MqttRoute("{IMEI}/GPS")]
    public void GPS(string IMEI) => logger.LogInformation("*** GPS Payload Received! IMEI: " + IMEI);

    [MqttRoute("{IMEI}/Image")]
    public void Image(string IMEI) => logger.LogInformation("*** Image Payload Received! IMEI: " + IMEI);

    [MqttRoute("{IMEI}/OBD")]
    public void OBD(string IMEI) => logger.LogInformation("*** OBD Payload Received! IMEI: " + IMEI);

    [MqttRoute("{IMEI}/Signal")]
    public void Signal(string IMEI) => logger.LogInformation("*** Signal Payload Received! IMEI: " + IMEI);

    [MqttRoute("{IMEI}/Temp")]
    public void Temp(string IMEI) => logger.LogInformation("*** Temperature Payload Received! IMEI: " + IMEI);

    [MqttRoute("{IMEI}/TPMS")]
    public void TPMS(string IMEI) => logger.LogInformation("*** TPMS Payload Received! IMEI: " + IMEI);

    [MqttRoute("{IMEI}/Voltage")]
    public void Voltage(string IMEI) => logger.LogInformation("*** Voltage Payload Received! IMEI: " + IMEI);
}