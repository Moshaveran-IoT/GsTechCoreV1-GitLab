using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;
using Moshaveran.Mqtt.DataAccess.Repositories.Bases;

namespace Moshaveran.Mqtt.DataAccess.Repositories;

internal sealed class CameraBrokerRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : BrokerRepositoryBase<CameraBroker>(readDbContext, writeDbContext)
{
    protected override Task SaveBrokerAsync(EntityEntry<CameraBroker> broker, CancellationToken cancellationToken = default) => Task.CompletedTask;
}