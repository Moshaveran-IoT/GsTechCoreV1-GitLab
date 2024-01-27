using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;
using Moshaveran.Mqtt.DataAccess.Repositories.Bases;

namespace Moshaveran.Mqtt.DataAccess.Repositories;

internal sealed class GpsBrokerRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : BrokerRepositoryBase<GpsBroker>(readDbContext, writeDbContext)
{
    protected override Task SaveBrokerAsync(EntityEntry<GpsBroker> broker, CancellationToken cancellationToken = default) =>
        this.WriteDbContext.SaveGpsBrokerAsync(broker, cancellationToken);
}