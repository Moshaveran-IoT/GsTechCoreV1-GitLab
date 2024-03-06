using Microsoft.Extensions.Logging;

using Moshaveran.Library.Data;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

namespace Moshaveran.GsTech.Mqtt.DataAccess.Repositories.Bases;

internal sealed class GenericRepository<TModel>(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext, ILogger<GenericRepository<TModel>> logger)
    : RepositoryBase<TModel, MqttReadDbContext, MqttWriteDbContext>(readDbContext, writeDbContext, logger) where TModel : class;