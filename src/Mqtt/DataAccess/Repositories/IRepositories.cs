using Moshaveran.Infrastructure.Results;

namespace Moshaveran.Mqtt.DataAccess.Repositories;

public interface IRepository<TModel>
{
    Task<Result> Delete(TModel model, bool persist = true, CancellationToken token = default);

    Task<Result> Insert(TModel model, bool persist = true, CancellationToken token = default);

    Task<Result> SaveChanges(CancellationToken token = default);

    Task<Result> Update(TModel model, bool persist = true, CancellationToken token = default);
}