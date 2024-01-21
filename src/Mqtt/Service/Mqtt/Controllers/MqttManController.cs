using Microsoft.AspNetCore.Mvc;
using MQTTnet.Server;

namespace Moshaveran.WinService.Mqtt.Controllers;

[Route("[controller]")]
[ApiController]
public class MqttManController(IMqttServer mqttServer) : ControllerBase
{
    [HttpGet("IsAlive")]
    public IActionResult IsAlive() =>
        mqttServer.IsStarted
            ? Ok("Working")
            : NotFound("Not working");
}