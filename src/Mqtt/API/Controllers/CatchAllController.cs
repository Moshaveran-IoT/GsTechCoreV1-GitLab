using System.Text;
using MQTTnet.AspNetCore.AttributeRouting;

namespace Moshaveran.API.Controllers;

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
