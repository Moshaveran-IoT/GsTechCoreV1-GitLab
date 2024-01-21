using Microsoft.AspNetCore.Mvc;

using MQTTnet.Server;

namespace Moshaveran.BackgroundServices.Mqtt.Controllers;

[Route("[controller]")]
[ApiController]
public class MqttManController(IMqttServer mqttServer) : ControllerBase
{
    [HttpGet("IsAlive")]
    public IActionResult IsAlive() =>
        mqttServer.IsStarted
            ? this.Ok("Working")
            : this.NotFound("Not working");
}