using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.GsTech.Mqtt.DataAccess.Repositories.Bases;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

namespace Moshaveran.GsTech.Mqtt.DataAccess.Repositories;

internal sealed class TemperatureBrokerRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : LastBrokerRepositoryBase<TemperatureBroker>(readDbContext, writeDbContext)
{
    protected override Task SaveBrokerAsync(EntityEntry<TemperatureBroker> broker, CancellationToken cancellationToken = default) =>
        this.WriteDbContext.SaveTemperatureBrokerAsync(broker, cancellationToken);
}