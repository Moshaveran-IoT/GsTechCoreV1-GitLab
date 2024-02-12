using Microsoft.Extensions.DependencyInjection;

using Moshaveran.Infrastructure;
using Moshaveran.Infrastructure.Mapping;

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
