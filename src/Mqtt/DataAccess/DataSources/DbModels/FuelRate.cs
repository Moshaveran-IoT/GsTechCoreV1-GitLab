using System;
using System.Collections.Generic;

namespace Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

public partial class FuelRate
{
    public int Id { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public int FuelTypeId { get; set; }

    public decimal LiterRate { get; set; }

    public decimal FreeLiterRate { get; set; }

    public string? Description { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public int? VehicleTypeId { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? DeleteOn { get; set; }

    public virtual FuelType FuelType { get; set; } = null!;

    public virtual VehicleType? VehicleType { get; set; }
}
