using System;
using System.Collections.Generic;

namespace Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

public partial class CanDailyTranslate
{
    public Guid Id { get; set; }

    public int? J1939id1 { get; set; }

    public long J1939id { get; set; }

    public Guid CanBrokerId { get; set; }

    public string? Value { get; set; }

    public string? Imei { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? DeleteOn { get; set; }

    public virtual CanDailyBroker CanBroker { get; set; } = null!;

    public virtual J1939? J1939id1Navigation { get; set; }
}
