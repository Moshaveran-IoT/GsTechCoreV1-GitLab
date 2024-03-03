using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Moshaveran.Library.Data;

public sealed class GenericRepository<TModel>(DbContext readDbContext, DbContext writeDbContext, ILogger<GenericRepository<TModel>> logger)
    : RepositoryBase<TModel, DbContext, DbContext>(readDbContext, writeDbContext, logger) where TModel : class
{
}