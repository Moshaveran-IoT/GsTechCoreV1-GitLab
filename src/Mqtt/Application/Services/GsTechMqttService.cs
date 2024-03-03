using Moshaveran.API.Mqtt.GrpcServices.Protos;
using Moshaveran.GsTech.Mqtt.Application.Interfaces;
using Moshaveran.Library.Data;
using Moshaveran.Library.Helpers;
using Moshaveran.Library.Results;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;
using Moshaveran.Mqtt.Domain.Services;

using System.Text;

namespace Moshaveran.GsTech.Mqtt.Application.Services;

public sealed class GsTechMqttService(
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
    IListenerService listenerService) : IGsTechMqttService
{
    private readonly IRepository<CameraBroker> _cameraRepo = cameraRepo;
    private readonly IRepository<CanBroker> _canRepo = canRepo;
    private readonly IRepository<GeneralBroker> _genRepo = genRepo;
    private readonly IGeocodingService _geocoding = geocoding;
    private readonly IRepository<GpsBroker> _gpsRepo = gpsRepo;
    private readonly IListenerService _listenerService = listenerService;
    private readonly IRepository<ObdBroker> _obdRepo = obdRepo;
    private readonly IRepository<SignalBroker> _signalRepo = signalRepo;
    private readonly IRepository<TemperatureBroker> _tempRepo = tempRepo;
    private readonly IRepository<TpmsBroker> _tpmsRepo = tpmsRepo;
    private readonly IRepository<VoltageBroker> _voltageRepo = voltageRepo;

    public Task<IResult> ProcessCameraPayload(ProcessPayloadArgs args)
        => this.Save((broker, payloadMessage) =>
        {
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            broker.Value = $"data:image/png;base64,{payloadMessage}";
            return IResult.Success(broker).ToAsync();
        }, args, this._cameraRepo);

    public Task<IResult> ProcessCanPayload(ProcessPayloadArgs args)
        => this.InnerSave(payloadMessage =>
        {
            IResult<IEnumerable<CanBroker>> result;
            if (!JsonDocumentHelpers.TryParse(payloadMessage, out var dto) || dto == null)
            {
                result = IResult.Fail<IEnumerable<CanBroker>>([]);
            }
            else
            {
                using (dto)
                {
                    result = IResult.Success(processDto(args.Imei, dto).Build());
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

                    yield return new CanBroker
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

    public Task<IResult> ProcessGeneralPayload(ProcessPayloadArgs args)
        => this.Save(broker =>
        {
            if (string.IsNullOrEmpty(broker.InternetTotalVolume))
            {
                return IResult<GeneralBroker>.Failed!;
            }
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            broker.InternetRemainingUssd = broker.InternetTotalVolume;
            var ussd = StringHelper.HexToUnicode(broker.InternetTotalVolume);
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
            return IResult.Success(broker);
        }, args, this._genRepo);

    public Task<IResult> ProcessGeneralPlusPayload(ProcessPayloadArgs args)
        => this.Save(broker =>
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
            return IResult.Success(broker);
        }, args, this._genRepo);

    public Task<IResult> ProcessGpsPayload(ProcessPayloadArgs args)
        => this.Save(async (broker, _) =>
        {
            if (broker.Latitude is >= -90 and <= 90 && broker.Longitude is >= -180 and <= 180 && broker.Latitude.ToString().Length != 1 && broker.Longitude.ToString().Length != 1)
            {
                broker.Imei = args.Imei;
                broker.CreatedOn = DateTime.Now;
                broker.Address = await this._geocoding.Reverse(broker.Latitude, broker.Longitude).GetValueAsync();
                return IResult.Success(broker);
            }
            else
            {
                return IResult.Fail(broker);
            }
        }, args, this._gpsRepo);

    public Task<IResult> ProcessObdPayload(ProcessPayloadArgs args)
        => this.Save((_, payloadMessage) =>
        {
            var broker = new ObdBroker
            {
                Imei = args.Imei,
                CreatedOn = DateTime.Now,
                Value = payloadMessage
            };
            return IResult.Success(broker).ToAsync();
        }, args, this._obdRepo);

    public Task<IResult> ProcessSignalPayload(ProcessPayloadArgs args)
        => this.Save(broker =>
        {
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            return IResult.Success(broker);
        }, args, this._signalRepo);

    public Task<IResult> ProcessTemperaturePayload(ProcessPayloadArgs args)
        => this.Save(broker =>
        {
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            return IResult.Success(broker);
        }, args, this._tempRepo);

    public Task<IResult> ProcessTpmsPayload(ProcessPayloadArgs args)
        => this.Save(broker =>
        {
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            return IResult.Success(broker);
        }, args, this._tpmsRepo);

    public Task<IResult> ProcessVoltagePayload(ProcessPayloadArgs args)
        => this.Save(broker =>
        {
            broker.Imei = args.Imei;
            broker.CreatedOn = DateTime.Now;
            return IResult.Success(broker);
        }, args, this._voltageRepo);

    private async Task<IResult> InnerSave<TDbBroker>(Func<string, Task<IResult<IEnumerable<TDbBroker>>>> initialize, ProcessPayloadArgs args, IRepository<TDbBroker> repo)
    {
        var (status, logMessage) = (SaveStatus.SaveSuccess, string.Empty);
        try
        {
            (var result, (status, logMessage)) = await save(initialize, args, repo);
            return result;
        }
        catch (Exception ex)
        {
            (status, logMessage) = (SaveStatus.SaveFailure, ex.GetBaseException().Message);
            return IResult.Failed;
        }
        finally
        {
            await sendToMonitor(args, status, logMessage);
        }

        static async Task<IResult<(SaveStatus Status, string LogMessage)>> save(Func<string, Task<IResult<IEnumerable<TDbBroker>>>> initialize, ProcessPayloadArgs args, IRepository<TDbBroker> repo)
        {
            (SaveStatus Status, string LogMessage) value;
            var payloadMessage = Encoding.UTF8.GetString(args.Payload);
            var initBrokers = await initialize(payloadMessage);
            if (initBrokers.IsSucceed && initBrokers.Value?.Any() is true)
            {
                foreach (var broker in initBrokers.Value)
                {
                    var insertResult = await repo.Insert(broker, false);
                    if (!insertResult.IsSucceed)
                    {
                        value = (SaveStatus.SaveFailure, insertResult.Message ?? "Payload cannot be saved.");
                        return insertResult.WithValue(value);
                    }
                }
                var saveResult = await repo.SaveChanges();

                value = saveResult.Process(onSucceed: r => (SaveStatus.SaveSuccess, r!.Message ?? "Payload is saved successfully.")
                                         , onFailure: r => (SaveStatus.SaveFailure, r!.Message ?? "Payload cannot be saved."));

                return saveResult.WithValue(value);
            }
            else
            {
                value = (SaveStatus.InvalidRequest, initBrokers.Message ?? "Invalid payload format.");
                return initBrokers.WithValue(value);
            }
        }

        Task sendToMonitor(ProcessPayloadArgs args, SaveStatus status, string logMessage)
            => this._listenerService.LogPayloadReceivedAsync(new LogPayloadReceivedArgs<TDbBroker>(args.ClientId, args.Imei, logMessage, status));
    }

    private async Task<IResult> Save<TDbBroker>(Func<TDbBroker, string, Task<IResult<TDbBroker>>> initialize, ProcessPayloadArgs args, IRepository<TDbBroker> repo)
        => await this.InnerSave(async payloadMessage =>
        {
            if (!StringHelper.TryParseJson(payloadMessage, out TDbBroker? broker) || broker == null)
            {
                return IResult.Fail<IEnumerable<TDbBroker>>([], "Invalid JSON format.");
            }
            var initResult = await initialize(broker, payloadMessage);
            return initResult.WithValue(EnumerableHelper.ToEnumerable(initResult.Value!));
        }, args, repo);

    private Task<IResult> Save<TDbBroker>(Func<TDbBroker, IResult<TDbBroker>> initialize, ProcessPayloadArgs args, IRepository<TDbBroker> repo)
        => this.Save((broker, _) => initialize(broker), args, repo);

    private Task<IResult> Save<TDbBroker>(Func<TDbBroker, string, IResult<TDbBroker>> initialize, ProcessPayloadArgs args, IRepository<TDbBroker> repo)
        => this.Save((broker, payloadMessage) => initialize(broker, payloadMessage), args, repo);
}