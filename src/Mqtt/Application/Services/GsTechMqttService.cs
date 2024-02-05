using System.Text;

using Application.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Moshaveran.API.Mqtt.GrpcServices.Protos;
using Moshaveran.Infrastructure.Helpers;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;
using Moshaveran.Mqtt.DataAccess.Repositories.Bases;
using Moshaveran.Mqtt.Domain.Services;

namespace Application.Services;

public sealed class GsTechMqttService : IGsTechMqttService
{
    private readonly IRepository<CameraBroker> _cameraRepo;
    private readonly IRepository<CanBroker> _canRepo;
    private readonly IConfiguration _configuration;
    private readonly IRepository<GeneralBroker> _genRepo;
    private readonly IGeocodingService _geocoding;
    private readonly IRepository<GpsBroker> _gpsRepo;
    private readonly IListenerService _listenerService;
    private readonly ILogger<GsTechMqttService> _logger;
    private readonly IRepository<ObdBroker> _obdRepo;
    private readonly IRepository<SignalBroker> _signalRepo;
    private readonly IRepository<TemperatureBroker> _tempRepo;
    private readonly IRepository<TpmsBroker> _tpmsRepo;
    private readonly IRepository<VoltageBroker> _voltageRepo;

    public GsTechMqttService(
        ILogger<GsTechMqttService> logger,
        IGeocodingService geocoding,
        IRepository<CanBroker> canRepo,
        IRepository<GeneralBroker> genRepo,
        IRepository<SignalBroker> signalRepo,
        IRepository<VoltageBroker> voltageRepo,
        IRepository<ObdBroker> obdRepo,
        IRepository<GpsBroker> gpsRepo,
        IRepository<TemperatureBroker> tempRepo,
        IRepository<TpmsBroker> tpmsRepo,
        IRepository<CameraBroker> cameraRepo,
        IConfiguration configuration,
        IListenerService listenerService)
    {
        this._configuration = configuration;
        this._listenerService = listenerService;
        this._cameraRepo = cameraRepo;
        this._canRepo = canRepo;
        this._genRepo = genRepo;
        this._geocoding = geocoding;
        this._gpsRepo = gpsRepo;
        this._logger = logger;
        this._obdRepo = obdRepo;
        this._signalRepo = signalRepo;
        this._tempRepo = tempRepo;
        this._tpmsRepo = tpmsRepo;
        this._voltageRepo = voltageRepo;
    }

    public Task<Result> ProcessCameraPayload(ProcessPayloadArgs args)
        => Save((broker, payloadMessage) =>
        {
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            broker.Value = $"data:image/png;base64,{payloadMessage}";
            return Result.CreateSucceed(broker);
        }, args, _cameraRepo);

    public Task<Result> ProcessCanPayload(ProcessPayloadArgs args)
        => this.InnerSave(payloadMessage =>
        {
            Result<IEnumerable<CanBroker>> result;
            if (!JsonDocumentHelpers.TryParse(payloadMessage, out var dto) || dto == null)
            {
                result = Result.CreateFailure<IEnumerable<CanBroker>>([]);
            }
            else
            {
                using (dto)
                {
                    result = Result.Create(processDto(args.Imei, dto).ToArray().AsEnumerable(), true);
                }
            }

            return Task.FromResult(result);

            static IEnumerable<CanBroker> processDto(string imei, System.Text.Json.JsonDocument dto)
            {
                const string VALID_HEX_CHARS = "0123456789abcdefABCDEF";
                foreach (var app in dto.RootElement.EnumerateObject())
                {
                    var (key, value) = (app.Name, app.Value.GetRawText().Trim('\"'));
                    var isHex = key.All(VALID_HEX_CHARS.Contains) && value.All(VALID_HEX_CHARS.Contains);
                    if (!isHex)
                    {
                        continue;
                    }
                    var binaryString = string.Concat(key.PadLeft(8, '0').Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
                    //x var Priority = binaryString[..6];
                    var reserved = binaryString[6..7];
                    var dataPage = binaryString[7..8];
                    var pduFormat = binaryString[8..16];
                    //x var sourceAddress = binaryString[24..32];
                    var decPduFormat = Convert.ToInt64(pduFormat, 2);
                    var pduSpecific = decPduFormat >= 240
                        ? binaryString[16..24]
                        : $"000000{reserved}{dataPage}{pduFormat}00000000";
                    var binaryPgn = $"000000{reserved}{dataPage}{pduFormat}{pduSpecific}";
                    var pgn = Convert.ToInt64(binaryPgn, 2);

                    yield return new CanBroker()
                    {
                        CreatedOn = DateTime.Now,
                        Identifier = key,
                        Pgn = pgn,
                        Value = value,
                        Imei = imei
                    };
                }
            }
        }, args, this._canRepo);

    public Task<Result> ProcessGeneralPayload(ProcessPayloadArgs args)
        => Save(broker =>
        {
            //if (string.IsNullOrEmpty(genBro.InternetTotalVolume))
            //{
            //    return;
            //}
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            broker.InternetRemainingUssd = broker.InternetTotalVolume;
            //var ussd = StringHelper.HexToUnicode(genBro.InternetTotalVolume);
            //if (ussd.Contains("صفر"))
            //{
            //    genBro.InternetTotalVolume = "بدون بسته";
            //    genBro.InternetRemainingTime = "---";
            //    var match = ussd.Split(["اصلی"], StringSplitOptions.None)[1].Split("ریال")[0].Trim().Split(" ")[0];
            //    genBro.InternetRemainingVolume = string.Concat(match, " ", "ریال");
            //}
            //else
            //{
            //    genBro.InternetTotalVolume = ussd.Split(":")[0].Trim();
            //    genBro.InternetRemainingVolume = ussd.Split(":")[1].Trim().Split("،")[0].Trim();
            //    genBro.InternetRemainingTime = ussd.Split(":")[1].Trim().Split("،")[1].Trim().Replace(".", "").Replace("تا", "");
            //}
            return Result.CreateSucceed(broker);
        }, args, _genRepo);

    public Task<Result> ProcessGeneralPlusPayload(ProcessPayloadArgs args)
        => Save(broker =>
        {
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            broker.InternetRemainingUssd = broker.InternetTotalVolume;
            var ussd = StringHelper.HexToUnicode(broker!.InternetTotalVolume!);
            if (ussd.Contains("صفر"))
            {
                broker.InternetTotalVolume = "بدون بسته";
                broker.InternetRemainingTime = "---";
                var match = ussd.Split(["اصلی"], StringSplitOptions.None)[1].Split("ریال")[0].Trim().Split(" ")[0];
                broker.InternetRemainingVolume = string.Concat(match, " ", "ریال");
            }
            else
            {
                broker.InternetTotalVolume = ussd.Split(":")[0].Trim();
                broker.InternetRemainingVolume = ussd.Split(":")[1].Trim().Split("،")[0].Trim();
                broker.InternetRemainingTime = ussd.Split(":")[1].Trim().Split("،")[1].Trim().Replace(".", "").Replace("تا", "");
            }

            if (!string.IsNullOrEmpty(broker.SimCardNumber))
            {
                var splitSimCard = StringHelper.HexToUnicode(broker.SimCardNumber).Split("\n");
                if (splitSimCard.ToList().Count > 0)
                {
                    broker.SimCardNumber = splitSimCard[1].Substring(2, 11);
                }
            }
            return Result.CreateSucceed(broker);
        }, args, _genRepo);

    public Task<Result> ProcessGpsPayload(ProcessPayloadArgs args)
        => Save(async (GpsBroker broker, string _) =>
        {
            if (broker.Latitude is >= -90 and <= 90 && broker.Longitude is >= -180 and <= 180 && broker.Latitude.ToString().Length != 1 && broker.Longitude.ToString().Length != 1)
            {
                broker.Imei = args.Imei;
                broker.CreatedOn = DateTime.Now;
                broker.Address = await _geocoding.Reverse(broker.Latitude, broker.Longitude);
                return Result.CreateSucceed(broker);
            }
            else
            {
                return Result.CreateFailure(broker);
            }
        }, args, _gpsRepo);

    public Task<Result> ProcessObdPayload(ProcessPayloadArgs args)
        => Save((ObdBroker _, string payloadMessage) =>
        {
            var broker = new ObdBroker
            {
                Imei = args.Imei,
                CreatedOn = DateTime.Now,
                Value = payloadMessage
            };
            return Result.CreateSucceed(broker);
        }, args, _obdRepo);

    public Task<Result> ProcessSignalPayload(ProcessPayloadArgs args)
        => Save(broker =>
        {
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            return Result.CreateSucceed(broker);
        }, args, _signalRepo);

    public Task<Result> ProcessTemperaturePayload(ProcessPayloadArgs args)
        => Save(broker =>
        {
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            return Result.CreateSucceed(broker);
        }, args, _tempRepo);

    public Task<Result> ProcessTpmsPayload(ProcessPayloadArgs args)
        => Save(broker =>
        {
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            return Result.CreateSucceed(broker);
        }, args, _tpmsRepo);

    public Task<Result> ProcessVoltagePayload(ProcessPayloadArgs args)
        => Save(broker =>
        {
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            return Result.CreateSucceed(broker);
        }, args, _voltageRepo);

    private async Task<Result> InnerSave<TDbBroker>(Func<string, Task<Result<IEnumerable<TDbBroker>>>> initialize, ProcessPayloadArgs args, IRepository<TDbBroker> repo)
    {
        var logMessage = string.Empty;
        var status = SaveStatus.SaveSuccess;
        try
        {
            var payloadMessage = Encoding.UTF8.GetString(args.Payload);
            var initBrokers = await initialize(payloadMessage);
            if (initBrokers.IsSucceed && initBrokers.Value?.Any() is true)
            {
                foreach (var broker in initBrokers.Value)
                {
                    var result = await repo.Insert(broker, false);
                    if (!result.IsSucceed)
                    {
                        status = SaveStatus.SaveFailure;
                        logMessage = result.Message ?? "Payload not saved";
                        return result;
                    }
                }
                var saveResult = await repo.SaveChanges();
                if (saveResult.IsSucceed)
                {
                    status = SaveStatus.SaveSuccess;
                    logMessage = saveResult.Message ?? "Payload saved successfully.";
                }
                else
                {
                    status = SaveStatus.SaveFailure;
                    logMessage = saveResult.Message ?? "Payload cannot be saved.";
                }
                return Result.Succeed;
            }
            else
            {
                status = SaveStatus.InvalidRequest;
                logMessage = initBrokers.Message ?? "Invalid payload or no payload received.";
                return initBrokers!;
            }
        }
        catch (Exception ex)
        {
            status = SaveStatus.SaveFailure;
            logMessage = ex.GetBaseException().Message;
            return Result.Failed;
        }
        finally
        {
            await this._listenerService.LogPayloadReceivedAsync(new LogPayloadReceivedArgs<TDbBroker>(args.ClientId, args.Imei, logMessage, status));
        }
    }

    private async Task<Result> InnerSave<TDbBroker>(Func<TDbBroker, string, Task<Result<TDbBroker>>> initialize, ProcessPayloadArgs args, IRepository<TDbBroker> repo)
        => await InnerSave(async p =>
        {
            if (!StringHelper.TryParseJson(p, out TDbBroker? broker) || broker == null)
            {
                return Result.CreateFailure<IEnumerable<TDbBroker>>([], "Invalid JSON format.");
            }
            var initResult = await initialize(broker, p);
            return initResult.WithValue(EnumerableHelper.ToEnumerable(initResult.Value!));
        }, args, repo);

    private async Task<Result> Save<TDbBroker>(Func<TDbBroker, string, Task<Result<TDbBroker>>> initialize, ProcessPayloadArgs args, IRepository<TDbBroker> repo) =>
        await InnerSave(initialize, args, repo);

    private Task<Result> Save<TDbBroker>(Func<TDbBroker, Result<TDbBroker>> initialize, ProcessPayloadArgs args, IRepository<TDbBroker> repo)
        => InnerSave((broker, _) =>
        {
            var result = initialize(broker);
            return Task.FromResult(result);
        }, args, repo);

    private Task<Result> Save<TDbBroker>(Func<TDbBroker, string, Result<TDbBroker>> initialize, ProcessPayloadArgs args, IRepository<TDbBroker> repo)
        => InnerSave((broker, payloadMessage) =>
        {
            var result = initialize(broker, payloadMessage);
            return Task.FromResult(result);
        }, args, repo);
}