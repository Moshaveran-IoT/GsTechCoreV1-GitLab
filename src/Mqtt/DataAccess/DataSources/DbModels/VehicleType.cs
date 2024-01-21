using System;
using System.Collections.Generic;

namespace Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

public partial class VehicleType
{
    public int Id { get; set; }

    public string? Image { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool IsValidAxis { get; set; }

    public bool IsValidObd { get; set; }

    public bool IsValidTempSensor { get; set; }

    public bool IsValidWieght { get; set; }

    public int NumberAxis { get; set; }

    public int NumberTempSensor { get; set; }

    public int NumberWieght { get; set; }

    public bool IsKilometerImage { get; set; }

    public int? TransportTypeId { get; set; }

    public int? UsageTypeId { get; set; }

    public bool IsValidCan { get; set; }

    public int MaxWeight { get; set; }

    public int MinWeight { get; set; }

    public bool IsDriverBehaviors { get; set; }

    public bool IsPeriodicService { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? DeleteOn { get; set; }

    public virtual ICollection<FuelRate> FuelRates { get; set; } = new List<FuelRate>();
}
