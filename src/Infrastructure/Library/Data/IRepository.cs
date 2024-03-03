using Moshaveran.Library.Results;

namespace Moshaveran.Library.Data;

public interface IRepository<TModel>
{
    ValueTask<IResult> Delete(TModel model, bool persist = true, CancellationToken cancellationToken = default);

    ValueTask<IList<TModel>> GetAllAsync(CancellationToken cancellationToken = default);

    ValueTask<TModel?> GetByIdAsync(int id, CancellationToken token = default);

    ValueTask<IResult> Insert(TModel model, bool persist = true, CancellationToken cancellationToken = default);

    ValueTask<IResult> SaveChanges(CancellationToken cancellationToken = default);

    ValueTask<IResult> Update(TModel model, bool persist = true, CancellationToken cancellationToken = default);
}