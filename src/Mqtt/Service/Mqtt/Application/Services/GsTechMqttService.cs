using System.Text;

using Moshaveran.Infrastructure.Helpers;
using Moshaveran.Infrastructure.Mapping;
using Moshaveran.Infrastructure.Results;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;
using Moshaveran.Mqtt.DataAccess.Repositories;

namespace Moshaveran.API.Mqtt.Application.Services;

public sealed class GsTechMqttService
{
    private readonly IRepository<CanBroker> _canRepo;
    private readonly IRepository<GeneralBroker> _genRepo;
    private readonly IMapper _mapper;

    public GsTechMqttService(IMapper mapper, IRepository<CanBroker> canRepo, IRepository<GeneralBroker> genRepo)
    {
        _mapper = mapper;
        _canRepo = canRepo;
        this._genRepo = genRepo;
    }

    public async Task<Result> ProcessCanPayload(byte[] payload, string imei, CancellationToken token = default)
    {
        var payloadMessage = Encoding.UTF8.GetString(payload);
        if (!JsonDocumentHelpers.TryParse(payloadMessage, out var dto))
        {
            return Result.Failed;
        }
        using (dto)
        {
            const string VALID_HEX = "0123456789abcdefABCDEF";
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
                    Value = rawValue,
                    Imei = imei
                };
                _ = await _canRepo.Insert(canBroker, false, token);
                var daily = _mapper.Map<CanDailyBroker>(canBroker);
                //await _mqttDbContext.CAN_Brokers.AddAsync(result);
                //await _mQTTCacheRepository.TranslateData(result, false);
                //var dailydata = JsonConvert.DeserializeObject<CAN_Daily_Broker>(JsonConvert.SerializeObject(result));
                //dailydata.Id = Guid.Empty;
                //await _mqttDbContext.CAN_Daily_Brokers.AddAsync(dailydata);
                //var TranslateData = JsonConvert.DeserializeObject<CAN_Broker>(JsonConvert.SerializeObject(dailydata));
                //await _mQTTCacheRepository.TranslateData(TranslateData, true);
            }
        }

        var result = await _canRepo.SaveChanges(token);
        return result;
    }

    public async Task<Result> ProcessGeneralPayload(byte[] payload, string imei, CancellationToken token = default)
    {
        var payloadMessage = Encoding.UTF8.GetString(payload);
        if (!StringHelper.TryParseJson(payloadMessage, out GeneralBroker? genBro) || genBro == null)
        {
            return await Task.FromResult(Result.Failed);
        }
        if (string.IsNullOrEmpty(genBro.InternetTotalVolume))
        {
            return await Task.FromResult(Result.Succeed);
        }
        genBro.Imei = imei;
        genBro.CreatedOn = DateTime.Now;
        genBro.InternetRemainingUssd = genBro.InternetTotalVolume;
        var ussd = StringHelper.HexToUnicode(genBro.InternetTotalVolume);
        if (ussd.Contains("صفر"))
        {
            genBro.InternetTotalVolume = "بدون بسته";
            genBro.InternetRemainingTime = "---";
            var match = ussd.Split(["اصلی"], StringSplitOptions.None)[1].Split("ریال")[0].Trim().Split(" ")[0];
            genBro.InternetRemainingVolume = string.Concat(match, " ", "ریال");
        }
        else
        {
            genBro.InternetTotalVolume = ussd.Split(":")[0].Trim();
            genBro.InternetRemainingVolume = ussd.Split(":")[1].Trim().Split("،")[0].Trim();
            genBro.InternetRemainingTime = ussd.Split(":")[1].Trim().Split("،")[1].Trim().Replace(".", "").Replace("تا", "");
        }
        //await _mqttDbContext.General_Brokers.AddAsync(result);
        //var dailydata = JsonConvert.DeserializeObject<General_Daily_Broker>(JsonConvert.SerializeObject(result));
        //dailydata.Id = Guid.Empty;
        //await _mqttDbContext.General_Daily_Brokers.AddAsync(dailydata);
        //await _mqttDbContext.SaveChangesAsync(CancellationToken.None);
        _ = await _genRepo.Insert(genBro, false, token);
        var result = await _genRepo.SaveChanges(token);
        var dailyData = _mapper.Map<GeneralDailyBroker>(result);
        return result;
    }

    public async Task<Result> ProcessGeneralPlusPayload(byte[] payload, string imei, CancellationToken token = default)
    {
        var payloadMessage = Encoding.UTF8.GetString(payload);
        if (!StringHelper.TryParseJson(payloadMessage, out GeneralBroker? result) || result == null)
        {
            return await Task.FromResult(Result.Failed);
        }
        if (string.IsNullOrEmpty(result.InternetTotalVolume))
        {
            return await Task.FromResult(Result.Succeed);
        }
        try
        {
            result.Imei = imei;
            result.CreatedOn = DateTime.Now;
            result.InternetRemainingUssd = result.InternetTotalVolume;
            var USSD = StringHelper.HexToUnicode(result.InternetTotalVolume);
            if (USSD.Contains("صفر"))
            {
                result.InternetTotalVolume = "بدون بسته";
                result.InternetRemainingTime = "---";
                var match = USSD.Split(["اصلی"], StringSplitOptions.None)[1].Split("ریال")[0].Trim().Split(" ")[0];
                result.InternetRemainingVolume = string.Concat(match, " ", "ریال");
            }
            else
            {
                result.InternetTotalVolume = USSD.Split(":")[0].Trim();
                result.InternetRemainingVolume = USSD.Split(":")[1].Trim().Split("،")[0].Trim();
                result.InternetRemainingTime = USSD.Split(":")[1].Trim().Split("،")[1].Trim().Replace(".", "").Replace("تا", "");
            }

            if (!string.IsNullOrEmpty(result.SimCardNumber))
            {
                var splitSimCard = StringHelper.HexToUnicode(result.SimCardNumber).Split("\n");
                if (splitSimCard.ToList().Count > 0)
                {
                    result.SimCardNumber = splitSimCard[1].ToString().Substring(2, 11);
                }
            }
            //await _mqttDbContext.General_Brokers.AddAsync(result);
            //var dailydata = JsonConvert.DeserializeObject<General_Daily_Broker>(JsonConvert.SerializeObject(result));
            //dailydata.Id = Guid.Empty;
            //await _mqttDbContext.General_Daily_Brokers.AddAsync(dailydata);
            ////await _restrictionRepository.GetRestrictionItems(imei, result, "General_Brokers");
            //await _mqttDbContext.SaveChangesAsync(CancellationToken.None);
            return await Task.FromResult(Result.Succeed);
        }
        catch
        {
            return await Task.FromResult(Result.Failed);
        }
    }

    public async Task<Result> ProcessSignalPayload(byte[] payload, string imei, CancellationToken token = default)
    {
        var payloadMessage = Encoding.UTF8.GetString(payload);
        if (!StringHelper.TryParseJson(payloadMessage, out SignalBroker? result) || result == null)
        {
            return await Task.FromResult(Result.Failed);
        }
        try
        {
            result.Imei = imei;
            result.CreatedOn = DateTime.Now;
            //await _mqttDbContext.Signal_Brokers.AddAsync(result);
            //var dailydata = JsonConvert.DeserializeObject<Signal_Daily_Broker>(JsonConvert.SerializeObject(result));
            //dailydata.Id = Guid.Empty;
            //await _mqttDbContext.Signal_Daily_Brokers.AddAsync(dailydata);
            //await _mqttDbContext.SaveChangesAsync(CancellationToken.None);
            return await Task.FromResult(Result.Succeed);
        }
        catch
        {
            return await Task.FromResult(Result.Failed);
        }
    }
}