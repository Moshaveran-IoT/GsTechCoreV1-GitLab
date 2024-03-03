using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moshaveran.GsTech.Mqtt.DataAccess.Repositories.Bases;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

namespace Moshaveran.GsTech.Mqtt.DataAccess.Repositories;

internal sealed class VoltageBrokerRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext, ILogger<VoltageBrokerRepository> logger)
    : LastBrokerRepositoryBase<VoltageBroker>(readDbContext, writeDbContext, logger)
{
    protected override async Task SaveBrokerAsync(EntityEntry<VoltageBroker> broker, CancellationToken token = default)
    {
        var addResult = await this.WriteDbContext.AddLastVoltageBrokerAsync(broker, token);

        // TODO Just for test
        broker.State = Microsoft.EntityFrameworkCore.EntityState.Detached;

        if (addResult.IsSucceed)
        {
            _ = await this.WriteDbContext.SaveChangesAsync(token);
        }
    }
}