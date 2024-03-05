using Moshaveran.Library.Results;

namespace Moshaveran.Library.Data;

public interface IRepository<TModel>
{
    Task<IResult> Delete(TModel model, bool persist = true, CancellationToken cancellationToken = default);

    Task<IList<TModel>> GetAll(CancellationToken cancellationToken = default);

    Task<TModel?> GetById(int id, CancellationToken token = default);

    Task<IResult> Insert(TModel model, bool persist = true, CancellationToken cancellationToken = default);

    Task<IResult> SaveChanges(CancellationToken cancellationToken = default);

    Task<IResult> Update(TModel model, bool persist = true, CancellationToken cancellationToken = default);
}