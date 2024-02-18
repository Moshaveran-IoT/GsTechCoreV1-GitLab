using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.GsTech.Mqtt.DataAccess.Repositories.Bases;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

namespace Moshaveran.GsTech.Mqtt.DataAccess.Repositories;

internal sealed class SignalBrokerRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : LastBrokerRepositoryBase<SignalBroker>(readDbContext, writeDbContext)
{
    protected override Task SaveBrokerAsync(EntityEntry<SignalBroker> broker, CancellationToken cancellationToken = default) =>
        this.WriteDbContext.SaveSignalBrokerAsync(broker, cancellationToken);
}