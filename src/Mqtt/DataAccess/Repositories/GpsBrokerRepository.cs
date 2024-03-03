using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moshaveran.GsTech.Mqtt.DataAccess.Repositories.Bases;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

namespace Moshaveran.GsTech.Mqtt.DataAccess.Repositories;

internal sealed class GpsBrokerRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext, ILogger<GpsBrokerRepository> logger)
    : LastBrokerRepositoryBase<GpsBroker>(readDbContext, writeDbContext, logger)
{
    protected override Task SaveBrokerAsync(EntityEntry<GpsBroker> broker, CancellationToken cancellationToken = default) =>
        this.WriteDbContext.SaveGpsBrokerAsync(broker, cancellationToken);
}