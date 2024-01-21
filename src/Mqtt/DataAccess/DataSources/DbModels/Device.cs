using System;
using System.Collections.Generic;

namespace Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

public partial class Device
{
    public int Id { get; set; }

    public string? Imei { get; set; }

    public string? SerialNumber { get; set; }

    public int ProductId { get; set; }

    public string? Version { get; set; }

    public string? Description { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? DeleteOn { get; set; }
}
