using System.Collections.Immutable;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Moshaveran.Library.Exceptions;
using Moshaveran.Library.Helpers;
using Moshaveran.Library.Validations;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

namespace Moshaveran.GsTech.Mqtt.DataAccess.Repositories.Bases;

internal abstract class LastBrokerRepositoryBase<TBroker>(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : GenericRepository<TBroker>(readDbContext, writeDbContext)
    where TBroker : class
{
    protected override async Task<Result> OnSavingChanges(CancellationToken token)
    {
        try
        {
            Check.MustBe<InvalidOperationGsTechException>(!this.WriteDbContext.ChangeTracker.Entries<TBroker>().Any(x => x.State != EntityState.Added));

            var entries = this.WriteDbContext.ChangeTracker.Entries<TBroker>().ToImmutableArray();
            await entries.Enumerate(this.SaveBrokerAsync, token);
            return Result.Succeed;
        }
        catch (GsTechExceptionBase ex)
        {
            return Result.Create(ex);
        }
    }

    protected abstract Task SaveBrokerAsync(EntityEntry<TBroker> broker, CancellationToken cancellationToken = default);
}