using System;
using System.Collections.Generic;

namespace DataAccess.DataEntities;

public partial class Fuel
{
    public int Id { get; set; }

    public int CustomerAssignmentId { get; set; }

    public string? Code { get; set; }

    public decimal LiterRate { get; set; }

    public decimal FreeLiterRate { get; set; }

    public string? CurrentKm { get; set; }

    public DateTime Date { get; set; }

    public string? Description { get; set; }

    public int FuelTypeId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public decimal FreeLiterAmount { get; set; }

    public decimal LiterAmount { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? DeleteOn { get; set; }

    public virtual FuelType FuelType { get; set; } = null!;
}
