using AutoMapper;

using Moshaveran.Infrastructure.Results;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;
using Moshaveran.Mqtt.DataAccess.Repositories;

using System.Text;
using System.Text.Json;

namespace Moshaveran.WinService.Mqtt.Services;

public sealed class GsTechMqttService(ILogger<GsTechMqttService> logger, IMapper mapper, IRepository<CanBroker> canRepo)
{
    private const string validHex = "0123456789abcdefABCDEF";

    public async Task<Result> InsertCanBroker(byte[] payload, string imei)
    {
        var payloadMessage = Encoding.UTF8.GetString(payload);
        var dto = JsonDocument.Parse(payloadMessage);
        foreach (var app in dto.RootElement.EnumerateObject())
        {
            var key = app.Name;
            var value = app.Value.GetRawText().Trim('\"');
            var isHex = key.All(validHex.Contains) && value.All(validHex.Contains);
            if (!isHex)
            {
                continue;
            }
            var binaryString = string.Join(string.Empty, key.PadLeft(8, '0').Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
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

            CanBroker canBroker = new()
            {
                CreatedOn = DateTime.Now,
                Identifier = key,
                Pgn = PGN,
                Value = app.Value.ToString(),
                Imei = imei
            };
            _ = await canRepo.Insert(canBroker, false);
            var daily = mapper.Map<CanDailyBroker>(canBroker);
            //await _mqttDbContext.CAN_Brokers.AddAsync(result);
            //await _mQTTCacheRepository.TranslateData(result, false);
            //var dailydata = JsonConvert.DeserializeObject<CAN_Daily_Broker>(JsonConvert.SerializeObject(result));
            //dailydata.Id = Guid.Empty;
            //await _mqttDbContext.CAN_Daily_Brokers.AddAsync(dailydata);
            //var TranslateData = JsonConvert.DeserializeObject<CAN_Broker>(JsonConvert.SerializeObject(dailydata));
            //await _mQTTCacheRepository.TranslateData(TranslateData, true);
        }
        var result = await canRepo.SaveChangesAsync();
        return result;
    }
}