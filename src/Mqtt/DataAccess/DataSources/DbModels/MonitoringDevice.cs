using System;
using System.Collections.Generic;

namespace Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

public partial class MonitoringDevice
{
    public string? Imei { get; set; }

    public string? UserName { get; set; }

    public string? UserImage { get; set; }

    public string? GeographicAreaName { get; set; }

    public string? GeographicAreaMap { get; set; }

    public string? VehicleName { get; set; }

    public string? VehicleImage { get; set; }

    public string? PlateNumber { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public float? Speed { get; set; }

    public float? Rotation { get; set; }

    public DateTime? GeneralBrokersLastUpdate { get; set; }

    public DateTime? GpsBrokersLastUpdate { get; set; }

    public DateTime? VoltageBrokersLastUpdate { get; set; }

    public DateTime? SignalBrokersLastUpdate { get; set; }

    public DateTime? TemperatureBrokersLastUpdate { get; set; }

    public DateTime? CanBrokersLastUpdate { get; set; }

    public DateTime? ObdBrokersLastUpdate { get; set; }

    public byte? Signal { get; set; }

    public int? Version { get; set; }

    public string? InternetRemainingTime { get; set; }

    public string? InternetRemainingVolume { get; set; }

    public bool? IsWeighting { get; set; }

    public float Capacity { get; set; }

    public bool? Fullig { get; set; }

    public decimal? Value { get; set; }

    public int? MaxWeight { get; set; }

    public int? MinWeight { get; set; }

    public int? VehicleId { get; set; }
}
