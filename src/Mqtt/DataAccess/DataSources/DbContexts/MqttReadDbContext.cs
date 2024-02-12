using Microsoft.EntityFrameworkCore;

namespace Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

internal sealed class MqttReadDbContext : MqttDbContext
{
    public MqttReadDbContext()
    {
    }

    public MqttReadDbContext(DbContextOptions<MqttDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => base.OnConfiguring(optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
}