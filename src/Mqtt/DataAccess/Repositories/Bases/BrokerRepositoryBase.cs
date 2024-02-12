using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.Library.Helpers;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

namespace Moshaveran.GsTech.Mqtt.DataAccess.Repositories.Bases;

internal abstract class BrokerRepositoryBase<TBroker>(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : GenericRepository<TBroker>(readDbContext, writeDbContext)
    where TBroker : class
{
    protected override async Task<Result> OnSavingChanges(CancellationToken token)
    {
        try
        {
            await this.WriteDbContext.ChangeTracker
                .Entries<TBroker>()
                .Enumerate(this.SaveBrokerAsync, token);
            return Result.Succeed;
        }
        catch (Exception ex)
        {
            return Result.Create(ex);
        }
    }

    protected abstract Task SaveBrokerAsync(EntityEntry<TBroker> broker, CancellationToken cancellationToken = default);
}