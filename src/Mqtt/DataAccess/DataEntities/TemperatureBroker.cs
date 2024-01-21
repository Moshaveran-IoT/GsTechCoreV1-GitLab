using System;
using System.Collections.Generic;

namespace DataAccess.DataEntities;

public partial class TemperatureBroker
{
    public Guid Id { get; set; }

    public string? Imei { get; set; }

    public string? Address { get; set; }

    public float Temperature { get; set; }

    public float? Humidity { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? DeleteOn { get; set; }
}
