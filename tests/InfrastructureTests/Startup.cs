using Microsoft.Extensions.DependencyInjection;

using Moshaveran.Library;
using Moshaveran.Library.Mapping;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureTests;
public sealed class Startup
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddInfrastructureService();
    }
}
