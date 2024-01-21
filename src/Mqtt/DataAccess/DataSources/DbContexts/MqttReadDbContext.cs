using Microsoft.EntityFrameworkCore;

namespace Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

public sealed class MqttWriteDbContext : MqttDbContext
{
    public MqttWriteDbContext()
    {
    }

    public MqttWriteDbContext(DbContextOptions<MqttDbContext> options) : base(options)
    {
    }
}

public sealed class MqttReadDbContext : MqttDbContext
{
    public MqttReadDbContext()
    {
    }

    public MqttReadDbContext(DbContextOptions<MqttDbContext> options) : base(options)
    {
    }
}