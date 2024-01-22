using System.Text;
using System.Text.Json;

using Moshaveran.Infrastructure.Mapping;
using Moshaveran.Infrastructure.Results;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;
using Moshaveran.Mqtt.DataAccess.Repositories;

namespace Moshaveran.API.Mqtt.Services;

public sealed class GsTechMqttService(ILogger<GsTechMqttService> logger, IMapper mapper, IRepository<CanBroker> canRepo)
{
    private const string VALID_HEX = "0123456789abcdefABCDEF";

    public async Task<Result> InsertCanBroker(byte[] payload, string imei)
    {
        var payloadMessage = Encoding.UTF8.GetString(payload);
        var dto = JsonDocument.Parse(payloadMessage);
        foreach (var app in dto.RootElement.EnumerateObject())
        {
            var key = app.Name;
            var rawValue = app.Value.GetRawText();
            var value = rawValue.Trim('\"');
            var isHex = key.All(VALID_HEX.Contains) && value.All(VALID_HEX.Contains);
            if (!isHex)
            {
                continue;
            }
            var binaryString = string.Join(string.Empty, key.PadLeft(8, '0').Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
            //var Priority = binaryString[..6];
            var reserved = binaryString[6..7];
            var dataPage = binaryString[7..8];
            var pduFormat = binaryString[8..16];
            //var sourceAddress = binaryString[24..32];
            var decPduFormat = Convert.ToInt64(pduFormat, 2);
            var pduSpecific = decPduFormat >= 240
                ? binaryString[16..24]
                : $"000000{reserved}{dataPage}{pduFormat}00000000";
            var binaryPgn = $"000000{reserved}{dataPage}{pduFormat}{pduSpecific}";
            var pgn = Convert.ToInt64(binaryPgn, 2);

            var canBroker = new CanBroker()
            {
                CreatedOn = DateTime.Now,
                Identifier = key,
                Pgn = pgn,
                Value = rawValue.ToString(),
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