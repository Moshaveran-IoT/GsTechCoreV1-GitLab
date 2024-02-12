using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.Infrastructure.Helpers;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

namespace Moshaveran.Mqtt.DataAccess.Repositories.Bases;

internal abstract class BrokerRepositoryBase<TBroker>(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : GenericRepository<TBroker>(readDbContext, writeDbContext)
    where TBroker : class
{
    protected override async Task<Result> OnSavingChanges(CancellationToken token)
    {
        try
        {
            await WriteDbContext.ChangeTracker
                .Entries<TBroker>()
                .Enumerate(SaveBrokerAsync, token);
            return Result.Succeed;
        }
        catch (Exception ex)
        {
            return Result.Create(ex);
        }
    }

    protected abstract Task SaveBrokerAsync(EntityEntry<TBroker> broker, CancellationToken cancellationToken = default);
}