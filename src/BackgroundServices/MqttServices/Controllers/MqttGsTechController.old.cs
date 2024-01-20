using Moshaveran.BackgroundServices.MqttServices.Services;

using MQTTnet.AspNetCore.AttributeRouting;

namespace Moshaveran.BackgroundServices.MqttServices.Controllers;

[MqttController]
[MqttRoute("Gs")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "<Pending>")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>")]
public sealed class MqttGsTechController(ILogger<MqttGsTechController> logger, GsTechMqttService mqttService) : MqttBaseController
{
    [MqttRoute("{IMEI}/CAN")]
    public void CAN(string IMEI) => logger.LogInformation("*** CAN Payload Received! IMEI: " + IMEI);

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