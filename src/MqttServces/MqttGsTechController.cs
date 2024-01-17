using Microsoft.Extensions.Logging;

using MQTTnet.AspNetCore.AttributeRouting;

using Newtonsoft.Json.Linq;

using System.Text;
using System.Text.RegularExpressions;

public class MqttTask
{
    public string Payload { get; set; }
    public string Topic { get; set; }
}

namespace GsTechCoreV1.Api.Controllers.MQTT.GsTech
{
    [MqttController]
    public class CatchAllController : MqttBaseController
    {
        private readonly ILogger<CatchAllController> _logger;

        public CatchAllController(ILogger<CatchAllController> logger)
        {
            _logger = logger;
        }

        [MqttRoute("{*topic}")]
        public Task WildCardMatchTopic(string topic)
        {
            //var payloadMessage = Encoding.UTF8.GetString(Message.Payload);
            _logger.LogInformation($"Wildcard matched on Topic: '{topic}'");
            //_logger.LogInformation($"{payloadMessage}");
            return Ok();
        }
    }

    [MqttController]
    [MqttRoute("Gs")]
    public class MqttGsTechController : MqttBaseController
    {
        //private readonly IMqttDbContext _mqttDbContext;
        private readonly ILogger<MqttGsTechController> _logger;

        //private readonly RestrictionRepository _restrictionRepository;

        //private readonly INeshanCacheRepository _neshanCacheRepository;
        //private readonly IWeatherCacheRepository _weatherCacheRepository;

        //private readonly IMQTTCacheRepository _mQTTCacheRepository;

        public MqttGsTechController(
            //IMqttDbContext mqttDbContext,
            ILogger<MqttGsTechController> logger
            //RestrictionRepository restrictionRepository,
            //INeshanCacheRepository neshanCacheRepository,
            //IWeatherCacheRepository weatherCacheRepository,
            //IMQTTCacheRepository mQTTCacheRepository
            )
        {
            //_mqttDbContext = mqttDbContext;
            _logger = logger;
            //_restrictionRepository = restrictionRepository;
            //_neshanCacheRepository = neshanCacheRepository;
            //_weatherCacheRepository = weatherCacheRepository;
            //_restrictionRepository.SetContext(_mqttDbContext);

            //_mQTTCacheRepository = mQTTCacheRepository;
            //_mQTTCacheRepository.SetContext(_mqttDbContext);
        }

        [MqttRoute("{IMEI}/General")]
        public async Task General(string IMEI)
        {
            _logger.LogInformation("*** General Payload Received! IMEI: " + IMEI);
            //var payloadMessage = Encoding.UTF8.GetString(Message.Payload);
            //if (payloadMessage.TryParseJson(out General_Broker result))
            //{
            //    result.IMEI = IMEI;
            //    result.CreatedOn = DateTime.Now;
            //    try
            //    {
            //        if (!String.IsNullOrEmpty(result.InternetTotalVolume))
            //        {
            //            result.InternetRemainingUSSD = result.InternetTotalVolume;
            //            string USSD = HexToUnicode(result.InternetTotalVolume);
            //            if (USSD.Contains("صفر"))
            //            {
            //                result.InternetTotalVolume = "بدون بسته";
            //                result.InternetRemainingTime = "---";
            //                var match = USSD.Split(new String[] { "اصلی" }, StringSplitOptions.None)[1].Split("ریال")[0].Trim().Split(" ")[0];
            //                result.InternetRemainingVolume = String.Concat(match, " ", "ریال");
            //            }
            //            else
            //            {
            //                result.InternetTotalVolume = USSD.Split(":")[0].Trim();
            //                result.InternetRemainingVolume = USSD.Split(":")[1].Trim().Split("،")[0].Trim();
            //                result.InternetRemainingTime = USSD.Split(":")[1].Trim().Split("،")[1].Trim().Replace(".", "").Replace("تا", "");
            //            }
            //        }
            //    }
            //    catch
            //    {
            //    }

            //    await _mqttDbContext.General_Brokers.AddAsync(result);
            //    var dailydata = JsonConvert.DeserializeObject<General_Daily_Broker>(JsonConvert.SerializeObject(result));
            //    dailydata.Id = Guid.Empty;
            //    await _mqttDbContext.General_Daily_Brokers.AddAsync(dailydata);

            //    await _mqttDbContext.SaveChangesAsync(CancellationToken.None);
                _logger.LogInformation("*** General Payload Saved! IMEI: " + IMEI);
            //}
        }

        [MqttRoute("{IMEI}/GeneralPlus")]
        public async Task GeneralPlus(string IMEI)
        {
            _logger.LogInformation("*** General Payload Received! IMEI: " + IMEI);
            //var payloadMessage = Encoding.UTF8.GetString(Message.Payload);
            //if (payloadMessage.TryParseJson(out General_Broker result))
            //{
                //result.IMEI = IMEI;
                //result.CreatedOn = DateTime.Now;
                //try
                //{
                //    if (!String.IsNullOrEmpty(result.InternetTotalVolume))
                //    {
                //        result.InternetRemainingUSSD = result.InternetTotalVolume;
                //        string USSD = HexToUnicode(result.InternetTotalVolume);
                //        if (USSD.Contains("صفر"))
                //        {
                //            result.InternetTotalVolume = "بدون بسته";
                //            result.InternetRemainingTime = "---";
                //            var match = USSD.Split(new String[] { "اصلی" }, StringSplitOptions.None)[1].Split("ریال")[0].Trim().Split(" ")[0];
                //            result.InternetRemainingVolume = String.Concat(match, " ", "ریال");
                //        }
                //        else
                //        {
                //            result.InternetTotalVolume = USSD.Split(":")[0].Trim();
                //            result.InternetRemainingVolume = USSD.Split(":")[1].Trim().Split("،")[0].Trim();
                //            result.InternetRemainingTime = USSD.Split(":")[1].Trim().Split("،")[1].Trim().Replace(".", "").Replace("تا", "");
                //        }
                //    }
                //    if (!String.IsNullOrEmpty(result.SimCardNumber))
                //    {
                //        var splitsimcard = HexToUnicode(result.SimCardNumber).Split("\n");
                //        if (splitsimcard.ToList().Count > 0)
                //        {
                //            result.SimCardNumber = splitsimcard[1].ToString().Substring(2, 11);
                //        }
                //    }
                //}
                //catch
                //{
                //}

                //await _mqttDbContext.General_Brokers.AddAsync(result);
                //var dailydata = JsonConvert.DeserializeObject<General_Daily_Broker>(JsonConvert.SerializeObject(result));
                //dailydata.Id = Guid.Empty;
                //await _mqttDbContext.General_Daily_Brokers.AddAsync(dailydata);
                //await _mqttDbContext.SaveChangesAsync(CancellationToken.None);
                _logger.LogInformation("*** General Payload Saved! IMEI: " + IMEI);
            //}
        }

        [MqttRoute("{IMEI}/Signal")]
        public async Task Signal(string IMEI)
        {
            _logger.LogInformation("*** Signal Payload Received! IMEI: " + IMEI);
            //var payloadMessage = Encoding.UTF8.GetString(Message.Payload);
            //if (payloadMessage.TryParseJson(out Signal_Broker result))
            //{
            //    result.IMEI = IMEI;
            //    result.CreatedOn = DateTime.Now;
            //    await _mqttDbContext.Signal_Brokers.AddAsync(result);
            //    var dailydata = JsonConvert.DeserializeObject<Signal_Daily_Broker>(JsonConvert.SerializeObject(result));
            //    dailydata.Id = Guid.Empty;
            //    await _mqttDbContext.Signal_Daily_Brokers.AddAsync(dailydata);
            //    await _mqttDbContext.SaveChangesAsync(CancellationToken.None);
            //    _logger.LogInformation("*** Signal Payload Saved! IMEI: " + IMEI);
            //}
        }

        [MqttRoute("{IMEI}/Voltage")]
        public async Task Voltage(string IMEI)
        {
            _logger.LogInformation("*** Voltage Payload Received! IMEI: " + IMEI);
            //var payloadMessage = Encoding.UTF8.GetString(Message.Payload);
            //if (payloadMessage.TryParseJson(out Voltage_Broker result))
            //{
            //    result.IMEI = IMEI;
            //    result.CreatedOn = DateTime.Now;
            //    await _mqttDbContext.Voltage_Brokers.AddAsync(result);
            //    var dailydata = JsonConvert.DeserializeObject<Voltage_Daily_Broker>(JsonConvert.SerializeObject(result));
            //    dailydata.Id = Guid.Empty;
            //    await _mqttDbContext.Voltage_Daily_Brokers.AddAsync(dailydata);
            //    await _mqttDbContext.SaveChangesAsync(CancellationToken.None);
                _logger.LogInformation("*** Voltage Payload Saved! IMEI: " + IMEI);
            //}
        }

        [MqttRoute("{IMEI}/GPS")]
        public async Task GPS(string IMEI)
        {
            //_logger.LogInformation("*** GPS Payload ! IMEI: " + IMEI + '\n' + "GPS Payload " + Encoding.UTF8.GetString(Message.Payload));
            _logger.LogInformation("*** GPS Payload Received! IMEI: " + IMEI);

            //var payloadMessage = Encoding.UTF8.GetString(Message.Payload);
            //if (payloadMessage.TryParseJson(out GPS_Broker result))
            //{
            //    if (-90 <= result.Latitude && result.Latitude <= 90 && -180 <= result.Longitude && result.Longitude <= 180 && (((int)result.Latitude).ToString().Length != 1
            //        && ((int)result.Longitude).ToString().Length != 1

            //        ))
            //    {
            //        result.IMEI = IMEI;
            //        result.CreatedOn = DateTime.Now;
            //        try
            //        {
            //            ReverseGeocoding nr = await _neshanCacheRepository.Reverse(result.Latitude, result.Longitude);
            //            result.Address = nr.Formatted_address;
            //        }
            //        catch (Exception e) { }
            //        await _mqttDbContext.GPS_Brokers.AddAsync(result);
            //        var dailydata = JsonConvert.DeserializeObject<GPS_Daily_Broker>(JsonConvert.SerializeObject(result));
            //        dailydata.Id = Guid.Empty;
            //        await _mqttDbContext.GPS_Daily_Brokers.AddAsync(dailydata);
            //        await _mqttDbContext.SaveChangesAsync(CancellationToken.None);

            //        _logger.LogInformation("*** GPS Payload Saved! IMEI: " + IMEI);
            //    }
            //    else
            //    {
                    _logger.LogInformation("*** GPS Payload Not Saved! IMEI: " + IMEI);
            //    }
            //}
        }

        [MqttRoute("{IMEI}/CAN")]
        public async Task CAN(string IMEI)
        {
            _logger.LogInformation("*** CAN Payload Received! IMEI: " + IMEI);
            //_logger.LogInformation("*** CAN Message: " + Encoding.UTF8.GetString(Message.Payload));
            //var payloadMessage = Encoding.UTF8.GetString(Message.Payload);
            //try
            //{
            //    var dto = JObject.Parse(payloadMessage);
            //    foreach (KeyValuePair<String, JToken> app in dto)
            //    {
            //        try
            //        {
            //            string validHex = "0123456789abcdefABCDEF";
            //            bool isHex = app.Key.All(validHex.Contains) && app.Value.ToString().All(validHex.Contains);
            //            if (isHex)
            //            {
            //                string binarystring = String.Join(String.Empty, app.Key.PadLeft(8, '0').Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
            //                var Priority = binarystring[..6];
            //                var Reserved = binarystring.Substring(6, 1);
            //                var DataPage = binarystring.Substring(7, 1);
            //                var PDUFormat = binarystring.Substring(8, 8);
            //                var PDUSpecific = binarystring.Substring(16, 8);
            //                var SourceAddress = binarystring.Substring(24, 8);
            //                var binaryPGN = "";
            //                var decPDUFormat = Convert.ToInt64(PDUFormat, 2);
            //                if (decPDUFormat >= 240)
            //                {
            //                    binaryPGN = String.Concat("000000", Reserved, DataPage, PDUFormat, PDUSpecific);
            //                }
            //                else
            //                {
            //                    binaryPGN = String.Concat("000000", Reserved, DataPage, PDUFormat, "00000000");
            //                }
            //                var PGN = Convert.ToInt64(binaryPGN, 2);
            //                CAN_Broker result = new();
            //                result.CreatedOn = DateTime.Now;
            //                result.Identifier = app.Key;
            //                result.PGN = PGN;
            //                result.Value = app.Value.ToString();
            //                result.IMEI = IMEI;
            //                await _mqttDbContext.CAN_Brokers.AddAsync(result);
            //                await _mQTTCacheRepository.TranslateData(result, false);
            //                var dailydata = JsonConvert.DeserializeObject<CAN_Daily_Broker>(JsonConvert.SerializeObject(result));
            //                dailydata.Id = Guid.Empty;
            //                await _mqttDbContext.CAN_Daily_Brokers.AddAsync(dailydata);
            //                var TranslateData = JsonConvert.DeserializeObject<CAN_Broker>(JsonConvert.SerializeObject(dailydata));
            //                await _mQTTCacheRepository.TranslateData(TranslateData, true);
            //            }
            //        }
            //        catch (Exception) { }
            //    }
            //}
            //catch (Exception) { }
            //await _mqttDbContext.SaveChangesAsync(CancellationToken.None);
            _logger.LogInformation("*** CAN Payload Saved! IMEI: " + IMEI);
        }

        [MqttRoute("{IMEI}/OBD")]
        public async Task OBD(string IMEI)
        {
            _logger.LogInformation("*** OBD Payload Received! IMEI: " + IMEI);
            //var payloadMessage = Encoding.UTF8.GetString(Message.Payload);

            //if (payloadMessage.TryParseJson(out Dictionary<string, string> data))
            //{
            //    var result = new OBD_Broker();
            //    result.IMEI = IMEI;
            //    result.CreatedOn = DateTime.Now;
            //    result.Value = payloadMessage;
            //    await _mqttDbContext.OBD_Brokers.AddAsync(result);
            //    var dailydata = JsonConvert.DeserializeObject<OBD_Daily_Broker>(JsonConvert.SerializeObject(result));
            //    dailydata.Id = Guid.Empty;
            //    await _mqttDbContext.OBD_Daily_Brokers.AddAsync(dailydata);

            //    await _mqttDbContext.SaveChangesAsync(CancellationToken.None);
                _logger.LogInformation("*** OBD Payload Saved! IMEI: " + IMEI);
            //}
        }

        [MqttRoute("{IMEI}/Temp")]
        public async Task Temp(string IMEI)
        {
            _logger.LogInformation("*** Temperature Payload Received! IMEI: " + IMEI);

            //var payloadMessage = Encoding.UTF8.GetString(Message.Payload);
            //if (payloadMessage.TryParseJson(out Temperature_Broker result))
            //{
            //    result.IMEI = IMEI;
            //    result.CreatedOn = DateTime.Now;
            //    await _mqttDbContext.Temperature_Brokers.AddAsync(result);
            //    var dailydata = JsonConvert.DeserializeObject<Temperature_Daily_Broker>(JsonConvert.SerializeObject(result));
            //    dailydata.Id = Guid.Empty;
            //    await _mqttDbContext.Temperature_Daily_Brokers.AddAsync(dailydata);

            //    await _mqttDbContext.SaveChangesAsync(CancellationToken.None);
                _logger.LogInformation("*** Temperature Payload Saved! IMEI: " + IMEI);
            //}

            //try
            //{
            //    await _restrictionRepository.GetRestrictionItems(IMEI, result, "Temperature_Brokers", result.CreatedOn);
            //}
            //catch (Exception e)
            //{
            //    _logger.LogError(e.Message + "*** Check Restriction Items Error! IMEI: " + IMEI);
            //}
        }

        [MqttRoute("{IMEI}/TPMS")]
        public async Task TPMS(string IMEI)
        {
            _logger.LogInformation("*** TPMS Payload Received! IMEI: " + IMEI);

            //var payloadMessage = Encoding.UTF8.GetString(Message.Payload);
            //if (payloadMessage.TryParseJson(out TPMS_Broker result))
            //{
            //    result.IMEI = IMEI;
            //    result.CreatedOn = DateTime.Now;
            //    await _mqttDbContext.TPMS_Brokers.AddAsync(result);
            //    var dailydata = JsonConvert.DeserializeObject<TPMS_Daily_Broker>(JsonConvert.SerializeObject(result));
            //    dailydata.Id = Guid.Empty;
            //    await _mqttDbContext.TPMS_Daily_Brokers.AddAsync(dailydata);

            //    await _mqttDbContext.SaveChangesAsync(CancellationToken.None);
                _logger.LogInformation("*** TPMS Payload Saved! IMEI: " + IMEI);
            //}
        }

        [MqttRoute("{IMEI}/Image")]
        public async Task Image(string IMEI)
        {
            //var payloadMessage = Convert.ToBase64String(Message.Payload);
            //Camera_Broker result = new Camera_Broker();
            //result.IMEI = IMEI;
            //result.CreatedOn = DateTime.Now;
            //result.Value = $"data:image/png;base64,{payloadMessage}";
            //await _mqttDbContext.Camera_Brokers.AddAsync(result);
            //var dailydata = JsonConvert.DeserializeObject<Camera_Daily_Broker>(JsonConvert.SerializeObject(result));
            //dailydata.Id = Guid.Empty;
            //await _mqttDbContext.Camera_Daily_Brokers.AddAsync(dailydata);

            //await _mqttDbContext.SaveChangesAsync(CancellationToken.None);
            _logger.LogInformation("*** Image Payload Received! IMEI: " + IMEI);
        }

        public string HexToUnicode(string hex)
        {
            Regex regex = new Regex(@"(?<hex>[0-9A-F]{2})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            byte[] bytes = regex.Matches(hex).OfType<Match>().Select(m => Convert.ToByte(m.Groups["hex"].Value, 16)).ToArray();
            char[] chars = Encoding.BigEndianUnicode.GetChars(bytes);
            var USSD = String.Join("", chars);
            return USSD;
        }
    }
}