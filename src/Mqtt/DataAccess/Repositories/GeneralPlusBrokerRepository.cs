using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

namespace Moshaveran.Mqtt.DataAccess.Repositories;

internal class GeneralPlusBrokerRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : BrokerRepositoryBase<GeneralBroker>(readDbContext, writeDbContext)
{
    protected override Task SaveBrokerAsync(EntityEntry<GeneralBroker> broker, CancellationToken cancellationToken = default) => 
        this.WriteDbContext.SaveGeneralBrokerAsync(broker, cancellationToken);
}