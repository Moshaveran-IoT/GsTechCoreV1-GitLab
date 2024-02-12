using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moshaveran.Infrastructure.Helpers;
public static class ObjectHelper
{
    public static string Pretty(object o)
    {
        var props = o.GetType().GetProperties();
        var sb = new StringBuilder();
        foreach (var prop in props)
        {
            sb.Append($"{prop.Name} : {prop.GetValue(o)}, ");
        }
        return sb.ToString().Trim().Trim(',');
    }
}
