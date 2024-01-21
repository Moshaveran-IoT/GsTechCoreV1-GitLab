using System;
using System.Collections.Generic;

namespace DataAccess.DataEntities;

public partial class FuelType
{
    public int Id { get; set; }

    public string? Key { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<FuelRate> FuelRates { get; set; } = new List<FuelRate>();

    public virtual ICollection<Fuel> Fuels { get; set; } = new List<Fuel>();
}
