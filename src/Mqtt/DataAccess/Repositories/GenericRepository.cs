using Moshaveran.Infrastructure.Results;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

namespace Moshaveran.Mqtt.DataAccess.Repositories;

public class GenericRepository<TModel> : IRepository<TModel>
{
    public GenericRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext)
        => (this.WriteDbContext, this.ReadDbContext) = (writeDbContext, readDbContext);

    protected MqttReadDbContext ReadDbContext { get; }
    protected MqttWriteDbContext WriteDbContext { get; }

    public Task<Result> Delete(TModel model, bool persist = true, CancellationToken token = default)
        => this.ManipulateModel(model, this.OnDeleting, persist, token);

    public Task<Result> Insert(TModel model, bool persist = true, CancellationToken token = default)
        => this.ManipulateModel(model, this.OnInserting, persist, token);

    public async Task<Result<int>> SaveChanges(CancellationToken token = default)
    {
        var result = await this.WriteDbContext.SaveChangesAsync(token);
        return Result.Create(result, result > 0);
    }

    public Task<Result> Update(TModel model, bool persist = true, CancellationToken token = default) =>
        this.ManipulateModel(model, this.OnUpdating, persist, token);

    protected virtual Task<Result> OnDeleting(TModel model, CancellationToken token)
    {
        _ = this.WriteDbContext.Remove(model!);
        return Task.FromResult(Result.Succeed);
    }

    protected virtual async Task<Result> OnInserting(TModel model, CancellationToken token = default)
    {
        _ = await this.WriteDbContext.AddAsync(model!, token);
        return Result.Succeed;
    }

    protected virtual Task<Result> OnUpdating(TModel model, CancellationToken token = default)
    {
        _ = this.WriteDbContext.Update(model!);
        return Task.FromResult(Result.Succeed);
    }

    protected virtual Result OnValidating(TModel? model, CancellationToken token = default)
        => Result.Succeed;

    private async Task<Result> ManipulateModel(TModel model, Func<TModel, CancellationToken, Task<Result>> action, bool persist = true, CancellationToken token = default)
    {
        var vr = this.OnValidating(model, token);
        if (vr?.IsSucceed != true)
        {
            return vr == null ? Result<int>.Failed : Result.Create<int>(default, vr);
        }

        var ir = await action(model, token);
        if (ir?.IsSucceed != true)
        {
            return ir == null ? Result<int>.Failed : Result.Create<int>(default, ir);
        }
        if (persist)
        {
            return await this.SaveChanges(token);
        };
        return Result<int>.Succeed;
    }
}