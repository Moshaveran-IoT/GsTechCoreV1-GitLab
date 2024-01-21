using System;
using System.Collections.Generic;

namespace DataAccess.DataEntities;

public partial class GpsDailyBroker
{
    public Guid Id { get; set; }

    public string? Imei { get; set; }

    public DateTime? GpsDateTime { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public float Speed { get; set; }

    public float Altitude { get; set; }

    public string? Address { get; set; }

    public string? Cloudiness { get; set; }

    public string? WindSpeed { get; set; }

    public string? Humidity { get; set; }

    public string? TemperatureValue { get; set; }

    public string? TemperatureHigh { get; set; }

    public string? TemperatureLow { get; set; }

    public string? Location { get; set; }

    public string? WeatherIcon { get; set; }

    public string? Weather { get; set; }

    public string? WeatherDescription { get; set; }

    public float? Angle { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? DeleteOn { get; set; }
}
