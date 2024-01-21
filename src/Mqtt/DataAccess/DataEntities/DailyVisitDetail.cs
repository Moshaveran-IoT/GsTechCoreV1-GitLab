using System;
using System.Collections.Generic;

namespace DataAccess.DataEntities;

public partial class DailyVisitDetail
{
    public int Id { get; set; }

    public int GoodId { get; set; }

    public string? Status { get; set; }

    public int DailyVisitId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool IsDelete { get; set; }

    public virtual DailyVisit DailyVisit { get; set; } = null!;
}
