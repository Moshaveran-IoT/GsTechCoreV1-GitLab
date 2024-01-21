using System;
using System.Collections.Generic;

namespace DataAccess.DataEntities;

public partial class CanTranslate
{
    public Guid Id { get; set; }

    public int? J1939id1 { get; set; }

    public long J1939id { get; set; }

    public Guid CanBrokerId { get; set; }

    public string? Value { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public string? Imei { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? DeleteOn { get; set; }
}
