using Moshaveran.Infrastructure.Results;

namespace Moshaveran.Mqtt.DataAccess.Repositories;

public interface IRepository<TModel>
{
    Task<Result<int>> Delete(Guid id, bool persist = true);

    Task<Result<int>> Insert(TModel model, bool persist = true);

    Task<Result<int>> SaveChangesAsync();

    Task<Result<int>> Update(TModel model, bool persist = true);
}