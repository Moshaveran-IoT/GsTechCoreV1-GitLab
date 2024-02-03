using System.Text;

using Google.Protobuf.WellKnownTypes;

using Grpc.Net.Client;

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
    private readonly MqqtReceiveSrvice.MqqtReceiveSrviceClient _grpcClient = default!;
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
        IConfiguration configuration)
    {
        _configuration = configuration;
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
        _grpcClient = createGrpcClient();

        MqqtReceiveSrvice.MqqtReceiveSrviceClient createGrpcClient()
        {
            var channel = GrpcChannel.ForAddress(_configuration["GrpcServiceSettings:ServerUrl"]!);
            return new MqqtReceiveSrvice.MqqtReceiveSrviceClient(channel);
        }
    }

    public Task<Result> ProcessCameraPayload(byte[] payload, string imei, CancellationToken token = default)
        => Save((broker, payloadMessage) =>
        {
            broker.Imei = imei;
            broker.CreatedOn = DateTime.Now;
            broker.Value = $"data:image/png;base64,{payloadMessage}";
            return Result.CreateSucceed(broker);
        }, "Camera", payload, _cameraRepo);

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
                var (key, value) = (app.Name, app.Value.GetRawText().Trim('\"'));
                var isHex = key.All(VALID_HEX.Contains) && value.All(VALID_HEX.Contains);
                if (!isHex)
                {
                    continue;
                }
                var binaryString = string.Concat(key.PadLeft(8, '0').Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
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
                    Value = value,
                    Imei = imei
                };
                _ = await _canRepo.Insert(canBroker, false, token);
            }
        }

        return await _canRepo.SaveChanges(token);
    }

    public Task<Result> ProcessGeneralPayload(byte[] payload, string imei, CancellationToken token = default)
        => Save(broker =>
        {
            //if (string.IsNullOrEmpty(genBro.InternetTotalVolume))
            //{
            //    return;
            //}
            broker.Imei = imei;
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
        }, "General", payload, _genRepo);

    public Task<Result> ProcessGeneralPlusPayload(byte[] payload, string imei, CancellationToken token = default)
        => Save(broker =>
        {
            broker.Imei = imei;
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
        }, "GeneralPlus", payload, _genRepo);

    public Task<Result> ProcessGpsPayload(byte[] payload, string imei, CancellationToken token = default)
        => Save(async (GpsBroker broker, string _) =>
        {
            if (broker.Latitude is >= -90 and <= 90 && broker.Longitude is >= -180 and <= 180 && broker.Latitude.ToString().Length != 1 && broker.Longitude.ToString().Length != 1)
            {
                broker.Imei = imei;
                broker.CreatedOn = DateTime.Now;
                broker.Address = await _geocoding.Reverse(broker.Latitude, broker.Longitude);
                return Result.CreateSucceed(broker);
            }
            else
            {
                return Result.CreateFailure(broker);
            }
        }, imei, payload, _gpsRepo);

    public Task<Result> ProcessObdPayload(byte[] payload, string imei, CancellationToken token = default)
        => Save((ObdBroker _, string payloadMessage) =>
        {
            var broker = new ObdBroker
            {
                Imei = imei,
                CreatedOn = DateTime.Now,
                Value = payloadMessage
            };
            return Result.CreateSucceed(broker);
        }, imei, payload, _obdRepo);

    public Task<Result> ProcessSignalPayload(byte[] payload, string imei, CancellationToken token = default)
        => Save(broker =>
        {
            broker.Imei = imei;
            broker.CreatedOn = DateTime.Now;
            return Result.CreateSucceed(broker);
        }, imei, payload, _signalRepo);

    public Task<Result> ProcessTemperaturePayload(byte[] payload, string imei, CancellationToken token = default)
        => Save(broker =>
        {
            broker.Imei = imei;
            broker.CreatedOn = DateTime.Now;
            return Result.CreateSucceed(broker);
        }, imei, payload, _tempRepo);

    public Task<Result> ProcessTpmsPayload(byte[] payload, string imei, CancellationToken token = default)
        => Save(broker =>
        {
            broker.Imei = imei;
            broker.CreatedOn = DateTime.Now;
            return Result.CreateSucceed(broker);
        }, imei, payload, _tpmsRepo);

    public Task<Result> ProcessVoltagePayload(byte[] payload, string imei, CancellationToken token = default)
        => Save(broker =>
        {
            broker.Imei = imei;
            broker.CreatedOn = DateTime.Now;
            return Result.CreateSucceed(broker);
        }, imei, payload, _voltageRepo);

    private async Task<Result> InnerSave<TDbBroker>(Func<TDbBroker, string, Task<Result<TDbBroker>>> initialize, string imei, byte[] payload, IRepository<TDbBroker> repo)
    {
        var payloadMessage = Encoding.UTF8.GetString(payload);
        if (!StringHelper.TryParseJson(payloadMessage, out TDbBroker? broker) || broker == null)
        {
            return await Task.FromResult(Result.Failed);
        }

        var grpcRequest = new PayloadReceivedParams
        {
            IMEI = imei,
            Time = Timestamp.FromDateTime(DateTime.UtcNow),
            BrokerType = typeof(TDbBroker).Name,
        };

        try
        {
            var initResult = await initialize(broker, payloadMessage);
            grpcRequest.SaveStatus = SaveStatus.SaveSuccess;
            grpcRequest.Log = "Hi";
            return initResult.IsSucceed ? await repo.Insert(initResult.Value!) : Result.Failed;
        }
        catch
        {
            grpcRequest.SaveStatus = SaveStatus.SaveFailure;
            grpcRequest.Log = "Bye";
            return Result.Failed;
        }
        finally
        {
            _ = await this._grpcClient.PayloadReceivedAsync(grpcRequest);
        }
    }

    private async Task<Result> Save<TDbBroker>(Func<TDbBroker, string, Task<Result<TDbBroker>>> initialize, string imei, byte[] payload, IRepository<TDbBroker> repo) =>
        await InnerSave(initialize, imei, payload, repo);

    private Task<Result> Save<TDbBroker>(Func<TDbBroker, Result<TDbBroker>> initialize, string imei, byte[] payload, IRepository<TDbBroker> repo)
        => InnerSave((broker, _) =>
        {
            var result = initialize(broker);
            return Task.FromResult(result);
        }, imei, payload, repo);

    private Task<Result> Save<TDbBroker>(Func<TDbBroker, string, Result<TDbBroker>> initialize, string imei, byte[] payload, IRepository<TDbBroker> repo) 
        => InnerSave((broker, payloadMessage) =>
        {
            var result = initialize(broker, payloadMessage);
            return Task.FromResult(result);
        }, imei, payload, repo);
}