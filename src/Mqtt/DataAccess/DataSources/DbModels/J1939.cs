using System;
using System.Collections.Generic;

namespace Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

public partial class J1939
{
    public int Id { get; set; }

    public string? Image { get; set; }

    public string? Name { get; set; }

    public string? Category { get; set; }

    public string? Label { get; set; }

    public bool Visibility { get; set; }

    public double? Min { get; set; }

    public double? Max { get; set; }

    public string? PostfixMetric { get; set; }

    public string? Comment { get; set; }

    public int Pgn { get; set; }

    public int StartBit { get; set; }

    public int BitLength { get; set; }

    public bool IsLittleEndian { get; set; }

    public bool IsSigned { get; set; }

    public double Factor { get; set; }

    public double Offset { get; set; }

    public string? DataType { get; set; }

    public bool Choking { get; set; }

    public int Interval { get; set; }

    public string? StatesKey { get; set; }

    public string? StatesValue { get; set; }

    public string? SourceUnit { get; set; }

    public string? PostfixImperial { get; set; }

    public int? MultiplexerValue { get; set; }

    public bool? IsMultiplexor { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public int? Distribution { get; set; }

    public int? Decimal { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? DeleteOn { get; set; }

    public virtual ICollection<CanDailyTranslate> CanDailyTranslates { get; set; } = new List<CanDailyTranslate>();

}
