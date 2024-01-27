using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;
using Moshaveran.Mqtt.DataAccess.DataSources.DbModels;

namespace Moshaveran.Mqtt.DataAccess.Repositories;
internal class SignalBrokerRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : BrokerRepositoryBase<SignalBroker>(readDbContext, writeDbContext)
{
    protected override Task SaveBrokerAsync(EntityEntry<SignalBroker> broker, CancellationToken cancellationToken = default) =>
        this.WriteDbContext.SaveSignalBrokerAsync(broker, cancellationToken);
}