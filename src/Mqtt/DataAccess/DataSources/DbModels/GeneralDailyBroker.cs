using System;
using System.Collections.Generic;

namespace Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

public partial class GeneralDailyBroker
{
    public Guid Id { get; set; }

    public string? Imei { get; set; }

    public int Version { get; set; }

    public string? SimCardNumber { get; set; }

    public byte SignalQuality { get; set; }

    public int? InputVoltage { get; set; }

    public int? BatteryVoltage { get; set; }

    public bool DhtBoardStatus { get; set; }

    public float? DhtBoardTemperature { get; set; }

    public float? DhtBoardHumidity { get; set; }

    public string? InternetRemainingVolume { get; set; }

    public string? InternetRemainingTime { get; set; }

    public string? InternetRemainingUssd { get; set; }

    public string? InternetTotalVolume { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? DeleteOn { get; set; }
}
