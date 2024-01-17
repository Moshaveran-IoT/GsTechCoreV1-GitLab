using Microsoft.AspNetCore.Mvc;

namespace BackgroundServices.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MqttController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> IsAlive()
    {
        await Task.Delay(3000);
        return Ok("Yes. MQTT Service is alive.");
    }
}