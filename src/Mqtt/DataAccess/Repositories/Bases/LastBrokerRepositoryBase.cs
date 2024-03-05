using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

using Moshaveran.Library.Data;
using Moshaveran.Library.Exceptions;
using Moshaveran.Library.Helpers;
using Moshaveran.Library.Validations;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

using System.Collections.Immutable;

namespace Moshaveran.GsTech.Mqtt.DataAccess.Repositories.Bases;

internal abstract class LastBrokerRepositoryBase<TBroker>(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext, ILogger logger)
    : RepositoryBase<TBroker, MqttReadDbContext, MqttWriteDbContext>(readDbContext, writeDbContext, logger) where TBroker : class
{
    protected override async Task<IResult> OnSavingChanges(CancellationToken token)
    {
        try
        {
            Check.MustBe<InvalidOperationGsTechException>(!this.WriteDbContext.ChangeTracker.Entries<TBroker>().Any(x => x.State != EntityState.Added));

            var entries = this.WriteDbContext.ChangeTracker.Entries<TBroker>().ToImmutableArray();
            await entries.Enumerate(this.SaveBrokerAsync, token);
            return IResult.Succeed;
        }
        catch (Exception ex)
        {
            return IResult.Fail(ex);
        }
    }

    protected abstract Task SaveBrokerAsync(EntityEntry<TBroker> broker, CancellationToken cancellationToken = default);
}