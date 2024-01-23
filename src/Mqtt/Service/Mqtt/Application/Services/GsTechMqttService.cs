using System.Text;

using Moshaveran.Infrastructure.ApplicationServices;
using Moshaveran.Infrastructure.Helpers;
using Moshaveran.Infrastructure.Mapping;
using Moshaveran.Infrastructure.Results;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;
using Moshaveran.Mqtt.DataAccess.Repositories;

namespace Moshaveran.API.Mqtt.Application.Services;

public sealed class GsTechMqttService : IBusinessService
{
    private readonly IRepository<CanBroker> _canRepo;
    private readonly IRepository<GeneralBroker> _genRepo;
    private readonly IGeocodingService _geocoding;
    private readonly IRepository<GpsBroker> _gpsRepo;
    private readonly ILogger<GsTechMqttService> _logger;
    private readonly IMapper _mapper;
    private readonly IRepository<SignalBroker> _signalRepo;
    private readonly IRepository<VoltageBroker> _voltageRepo;

    public GsTechMqttService(
        ILogger<GsTechMqttService> logger,
        IMapper mapper,
        IGeocodingService geocoding,
        IRepository<CanBroker> canRepo,
        IRepository<GeneralBroker> genRepo,
        IRepository<SignalBroker> signalRepo,
        IRepository<VoltageBroker> voltageRepo,
        IRepository<GpsBroker> gpsRepo)
    {
        this._logger = logger;
        this._mapper = mapper;
        this._geocoding = geocoding;
        this._canRepo = canRepo;
        this._genRepo = genRepo;
        this._signalRepo = signalRepo;
        this._voltageRepo = voltageRepo;
        this._gpsRepo = gpsRepo;
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
                var daily = _mapper.Map<CanDailyBroker>(canBroker).With(x => x.Id = Guid.Empty);
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

    public Task<Result> ProcessGeneralPayload(byte[] payload, string imei, CancellationToken token = default)
        => ProcessPayload<GeneralBroker>(async genBro =>
        {
            if (string.IsNullOrEmpty(genBro.InternetTotalVolume))
            {
                return;
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
            var dailyData = _mapper.Map<GeneralDailyBroker>(genBro).With(x => x.Id = Guid.Empty);
        }, payload);

    public Task<Result> ProcessGeneralPlusPayload(byte[] payload, string imei, CancellationToken token = default)
        => ProcessPayload(async (GeneralBroker gp) =>
        {
            gp.Imei = imei;
            gp.CreatedOn = DateTime.Now;
            gp.InternetRemainingUssd = gp.InternetTotalVolume;
            var USSD = StringHelper.HexToUnicode(gp.InternetTotalVolume);
            if (USSD.Contains("صفر"))
            {
                gp.InternetTotalVolume = "بدون بسته";
                gp.InternetRemainingTime = "---";
                var match = USSD.Split(["اصلی"], StringSplitOptions.None)[1].Split("ریال")[0].Trim().Split(" ")[0];
                gp.InternetRemainingVolume = string.Concat(match, " ", "ریال");
            }
            else
            {
                gp.InternetTotalVolume = USSD.Split(":")[0].Trim();
                gp.InternetRemainingVolume = USSD.Split(":")[1].Trim().Split("،")[0].Trim();
                gp.InternetRemainingTime = USSD.Split(":")[1].Trim().Split("،")[1].Trim().Replace(".", "").Replace("تا", "");
            }

            if (!string.IsNullOrEmpty(gp.SimCardNumber))
            {
                var splitSimCard = StringHelper.HexToUnicode(gp.SimCardNumber).Split("\n");
                if (splitSimCard.ToList().Count > 0)
                {
                    gp.SimCardNumber = splitSimCard[1].ToString().Substring(2, 11);
                }
            }
            //await _mqttDbContext.General_Brokers.AddAsync(result);
            //var dailydata = JsonConvert.DeserializeObject<General_Daily_Broker>(JsonConvert.SerializeObject(result));
            //dailydata.Id = Guid.Empty;
            //await _mqttDbContext.General_Daily_Brokers.AddAsync(dailydata);
            ////await _restrictionRepository.GetRestrictionItems(imei, result, "General_Brokers");
            //await _mqttDbContext.SaveChangesAsync(CancellationToken.None);
            _ = await this._genRepo.Insert(gp);
        }, payload);

    public Task<Result> ProcessGpsPayload(byte[] payload, string imei, CancellationToken token = default)
        => ProcessPayload(async (GpsBroker gps) =>
        {
            if ((gps.Latitude is >= -90 and <= 90) && (gps.Longitude is >= -180 and <= 180) && (gps.Latitude.ToString().Length != 1) && (gps.Longitude.ToString().Length != 1))
            {
                gps.Imei = imei;
                gps.CreatedOn = DateTime.Now;
                //ReverseGeocoding nr = await _geocoding.Reverse(result.Latitude, result.Longitude);
                //result.Address = nr.Formatted_address;
                var address = await _geocoding.Reverse(gps.Latitude, gps.Longitude);
                gps.Address = address;
                //await _mqttDbContext.GPS_Brokers.AddAsync(result);
                //var dailydata = JsonConvert.DeserializeObject<GPS_Daily_Broker>(JsonConvert.SerializeObject(result));
                //dailydata.Id = Guid.Empty;
                //await _mqttDbContext.GPS_Daily_Brokers.AddAsync(dailydata);
                //await _mqttDbContext.SaveChangesAsync(CancellationToken.None);

                //var dailyData = _mapper.Map<GpsDailyBroker>(result).With(x => x.Id = Guid.Empty);

                _ = await _gpsRepo.Insert(gps);
                _logger.LogInformation($"*** GPS Payload Saved! IMEI: {imei}");
            }
            else
            {
                _logger.LogInformation($"*** GPS Payload Not Saved! IMEI: {imei}");
            }
        }, payload);

    public Task<Result> ProcessSignalPayload(byte[] payload, string imei, CancellationToken token = default)
            => ProcessPayload(async (SignalBroker signal) =>
        {
            signal.Imei = imei;
            signal.CreatedOn = DateTime.Now;
            //await _mqttDbContext.Signal_Brokers.AddAsync(result);
            //var dailydata = JsonConvert.DeserializeObject<Signal_Daily_Broker>(JsonConvert.SerializeObject(result));
            //dailydata.Id = Guid.Empty;
            //await _mqttDbContext.Signal_Daily_Brokers.AddAsync(dailydata);
            //await _mqttDbContext.SaveChangesAsync(CancellationToken.None);
            _ = await _signalRepo.Insert(signal, token: token);
        }, payload);

    public Task<Result> ProcessVoltagePayload(byte[] payload, string imei, CancellationToken token = default)
        => ProcessPayload(async (VoltageBroker voltage) =>
        {
            voltage.Imei = imei;
            voltage.CreatedOn = DateTime.Now;
            //await _mqttDbContext.Voltage_Brokers.AddAsync(result);
            //var dailydata = JsonConvert.DeserializeObject<Voltage_Daily_Broker>(JsonConvert.SerializeObject(result));
            //dailydata.Id = Guid.Empty;
            //await _mqttDbContext.Voltage_Daily_Brokers.AddAsync(dailydata);
            //await _mqttDbContext.SaveChangesAsync(CancellationToken.None);
            _ = await _voltageRepo.Insert(voltage);
        }, payload);

    private static async Task<Result> ProcessPayload<TDbBroker>(Func<TDbBroker, Task> process, byte[] payload)
    {
        var payloadMessage = Encoding.UTF8.GetString(payload);
        if (!StringHelper.TryParseJson(payloadMessage, out TDbBroker? result) || result == null)
        {
            return await Task.FromResult(Result.Failed);
        }
        try
        {
            await process(result);
            return await Task.FromResult(Result.Succeed);
        }
        catch
        {
            return await Task.FromResult(Result.Failed);
        }
    }
}