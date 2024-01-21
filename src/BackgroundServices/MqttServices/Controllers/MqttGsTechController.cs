using Moshaveran.BackgroundServices.MqttServices.Services;

using MQTTnet.AspNetCore.AttributeRouting;

using System.Text;

namespace Moshaveran.BackgroundServices.MqttServices.Controllers;

[MqttController]
public class CatchAllController(ILogger<CatchAllController> logger) : MqttBaseController
{
    [MqttRoute("{*topic}")]
    public Task WildCardMatchTopic(string topic)
    {
        var payloadMessage = Encoding.UTF8.GetString(this.Message.Payload);
        logger.LogInformation($"Wildcard matched on Topic: '{topic}'");
        logger.LogInformation($"{payloadMessage}");
        return this.Ok();
    }
}

[MqttController]
[MqttRoute("Gs")]
public class MqttGsTechController(ILogger<MqttGsTechController> logger, GsTechMqttService service) : MqttBaseController
{
    [MqttRoute("{IMEI}/CAN")]
    public void CAN(string IMEI)
    {
        logger.LogInformation("*** CAN Payload Received! IMEI: " + IMEI);
        service.Can(Message.Payload, IMEI);
    }

    [MqttRoute("{IMEI}/General")]
    public void General(string IMEI)
    {
        logger.LogInformation("*** General Payload Received! IMEI: " + IMEI);

    }

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