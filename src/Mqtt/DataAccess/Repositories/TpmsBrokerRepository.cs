using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.GsTech.Mqtt.DataAccess.Repositories.Bases;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

namespace Moshaveran.GsTech.Mqtt.DataAccess.Repositories;

internal sealed class TpmsBrokerRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : LastBrokerRepositoryBase<TpmsBroker>(readDbContext, writeDbContext)
{
    protected override Task SaveBrokerAsync(EntityEntry<TpmsBroker> broker, CancellationToken cancellationToken = default) =>
        this.WriteDbContext.SaveTpmsBrokerAsync(broker, cancellationToken);
}