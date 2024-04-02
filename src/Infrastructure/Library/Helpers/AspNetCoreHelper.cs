using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moshaveran.Library.Helpers;
public static class AspNetCoreHelper
{
    public static string? GetEnvironment()
    {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }
}
