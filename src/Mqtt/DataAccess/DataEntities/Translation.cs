using System;
using System.Collections.Generic;

namespace DataAccess.DataEntities;

public partial class Translation
{
    public int Id { get; set; }

    public int LanguageId { get; set; }

    public int LanguageKeyId { get; set; }

    public string? Value { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? LastModifiedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool IsDelete { get; set; }

    public DateTime? DeleteOn { get; set; }
}
