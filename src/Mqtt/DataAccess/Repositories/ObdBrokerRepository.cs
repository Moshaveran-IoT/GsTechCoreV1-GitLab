using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moshaveran.GsTech.Mqtt.DataAccess.Repositories.Bases;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

namespace Moshaveran.GsTech.Mqtt.DataAccess.Repositories;

internal sealed class ObdBrokerRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext, ILogger<ObdBrokerRepository> logger)
    : LastBrokerRepositoryBase<ObdBroker>(readDbContext, writeDbContext, logger)
{
    protected override Task SaveBrokerAsync(EntityEntry<ObdBroker> broker, CancellationToken cancellationToken = default) =>
        this.WriteDbContext.SaveObdBrokerAsync(broker, cancellationToken);
}