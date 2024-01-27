using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;
using Moshaveran.Mqtt.DataAccess.Repositories.Bases;

namespace Moshaveran.Mqtt.DataAccess.Repositories;

internal sealed class ObdBrokerRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : BrokerRepositoryBase<ObdBroker>(readDbContext, writeDbContext)
{
    protected override Task SaveBrokerAsync(EntityEntry<ObdBroker> broker, CancellationToken cancellationToken = default) =>
        this.WriteDbContext.SaveObdBrokerAsync(broker, cancellationToken);
}