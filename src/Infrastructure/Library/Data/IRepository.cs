using Moshaveran.Library.Results;

namespace Moshaveran.Library.Data;

public interface IRepository<TModel>
{
    Task<IResult> Delete(TModel model, bool persist = true, CancellationToken token = default);

    Task<IResult> Insert(TModel model, bool persist = true, CancellationToken token = default);

    Task<IResult> SaveChanges(CancellationToken token = default);

    Task<IResult> Update(TModel model, bool persist = true, CancellationToken token = default);
}