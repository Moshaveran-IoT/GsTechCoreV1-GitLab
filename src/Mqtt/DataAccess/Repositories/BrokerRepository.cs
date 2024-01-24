using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.Infrastructure.Helpers;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

namespace Moshaveran.Mqtt.DataAccess.Repositories;

public abstract class BrokerRepositoryBase<TBroker> : GenericRepository<TBroker>
    where TBroker : class
{
    protected BrokerRepositoryBase(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : base(readDbContext, writeDbContext)
    {
    }

    protected override async Task<Result> OnSavingChanges(CancellationToken token)
    {
        //return base.OnSavingChanges(token);

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