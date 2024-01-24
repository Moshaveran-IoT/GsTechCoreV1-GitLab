using Microsoft.EntityFrameworkCore;

using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

namespace Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

public sealed class MqttReadDbContext : MqttDbContext
{
    public MqttReadDbContext()
    {
    }

    public MqttReadDbContext(DbContextOptions<MqttDbContext> options) : base(options)
    {
    }
}