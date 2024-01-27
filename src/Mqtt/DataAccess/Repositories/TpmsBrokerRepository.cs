using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;
using Moshaveran.Mqtt.DataAccess.Repositories.Bases;

namespace Moshaveran.Mqtt.DataAccess.Repositories;

internal sealed class TpmsBrokerRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : BrokerRepositoryBase<TpmsBroker>(readDbContext, writeDbContext)
{
    protected override Task SaveBrokerAsync(EntityEntry<TpmsBroker> broker, CancellationToken cancellationToken = default) =>
        this.WriteDbContext.SaveTpmsBrokerAsync(broker, cancellationToken);
}