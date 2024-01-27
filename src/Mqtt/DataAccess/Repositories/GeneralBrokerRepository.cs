using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;
using Moshaveran.Mqtt.DataAccess.Repositories.Bases;

namespace Moshaveran.Mqtt.DataAccess.Repositories;

internal class GeneralBrokerRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : BrokerRepositoryBase<GeneralBroker>(readDbContext, writeDbContext)
{
    protected override Task SaveBrokerAsync(EntityEntry<GeneralBroker> broker, CancellationToken cancellationToken = default) => 
        this.WriteDbContext.SaveGeneralBrokerAsync(broker, cancellationToken);
}