using Moshaveran.Infrastructure.Results;

namespace Moshaveran.Mqtt.DataAccess.Repositories;

public interface IRepository<TModel>
{
    Task<Result<int>> Delete(TModel model, bool persist = true, CancellationToken token = default);

    Task<Result<int>> Insert(TModel model, bool persist = true, CancellationToken token = default);

    Task<Result<int>> SaveChanges(CancellationToken token = default);

    Task<Result<int>> Update(TModel model, bool persist = true, CancellationToken token = default);
}