using System;
using System.Collections.Generic;

namespace DataAccess.DataEntities;

public partial class VoltageDailyBroker
{
    public Guid Id { get; set; }

    public string? Imei { get; set; }

    public int? InputVoltage { get; set; }

    public int? BatteryVoltage { get; set; }

    public bool DhtBoardStatus { get; set; }

    public float? DhtBoardTemperature { get; set; }

    public float? DhtBoardHumidity { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? DeleteOn { get; set; }
}
