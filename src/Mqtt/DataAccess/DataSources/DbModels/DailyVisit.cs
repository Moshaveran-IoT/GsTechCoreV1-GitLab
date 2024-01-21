using System;
using System.Collections.Generic;

namespace Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

public partial class DailyVisit
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public int CustomerAssignmentId { get; set; }

    public DateTime Date { get; set; }

    public string? Description { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? DeleteOn { get; set; }

    public virtual ICollection<DailyVisitDetail> DailyVisitDetails { get; set; } = new List<DailyVisitDetail>();
}
