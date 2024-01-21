using Newtonsoft.Json;

using System.Text;
using System.Text.Json;

namespace Moshaveran.BackgroundServices.MqttServices.Services;

public sealed class GsTechMqttService(ILogger<GsTechMqttService> logger)
{
    private const string validHex = "0123456789abcdefABCDEF";

    public Task Can(byte[] payload, string imei)
    {
        var payloadMessage = Encoding.UTF8.GetString(payload);
        var dto = JsonDocument.Parse(payloadMessage);
        foreach (var app in dto.RootElement.EnumerateObject())
        {
            var isHex = app.Name.All(validHex.Contains) && app.Value.GetRawText().All(validHex.Contains);
            if (!isHex)
            {
                continue;
            }
            var binaryString = string.Join(string.Empty, app.Name.PadLeft(8, '0').Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
            var Priority = binaryString[..6];
            var Reserved = binaryString[6..7];
            var DataPage = binaryString[7..8];
            var PDUFormat = binaryString[8..16];
            var SourceAddress = binaryString[24..32];
            var decPDUFormat = Convert.ToInt64(PDUFormat, 2);
            var PDUSpecific = decPDUFormat >= 240
                ? binaryString[16..24]
                : $"000000{Reserved}{DataPage}{PDUFormat}00000000";
            var binaryPGN = $"000000{Reserved}{DataPage}{PDUFormat}{PDUSpecific}";
            var PGN = Convert.ToInt64(binaryPGN, 2);
        }

        return Task.CompletedTask;
    }

    public async Task OBD(byte[] payload, string imei)
    {
        var payloadMessage = Encoding.UTF8.GetString(payload);

        
    }
}