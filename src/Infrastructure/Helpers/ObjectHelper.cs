using System.Text;

namespace Moshaveran.Infrastructure.Helpers;

public static class ObjectHelper
{
    public static string Pretty(object? o, char separator = ',')
        => o is null
            ? string.Empty
            : new StringBuilder()
                .AppendAll(o.GetType().GetProperties().OrderBy(x => x.Name).Select(x => $"{x.Name} : {x.GetValue(o)}{separator} "))
                .ToString().Trim().Trim(separator);
}