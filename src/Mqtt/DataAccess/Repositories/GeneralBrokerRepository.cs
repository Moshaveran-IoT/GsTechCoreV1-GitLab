using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moshaveran.GsTech.Mqtt.DataAccess.Repositories.Bases;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

namespace Moshaveran.GsTech.Mqtt.DataAccess.Repositories;

internal class GeneralBrokerRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext, ILogger<GeneralBrokerRepository> logger)
    : LastBrokerRepositoryBase<GeneralBroker>(readDbContext, writeDbContext, logger)
{
    protected override Task SaveBrokerAsync(EntityEntry<GeneralBroker> broker, CancellationToken cancellationToken = default) =>
        this.WriteDbContext.SaveGeneralBrokerAsync(broker, cancellationToken);
}