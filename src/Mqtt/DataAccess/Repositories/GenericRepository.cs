using Moshaveran.Infrastructure.Results;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

namespace Moshaveran.Mqtt.DataAccess.Repositories;

public class GenericRepository<TModel> : IRepository<TModel>
{
    public GenericRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext)
        => (this.WriteDbContext, this.ReadDbContext) = (writeDbContext, readDbContext);

    protected MqttReadDbContext ReadDbContext { get; }
    protected MqttWriteDbContext WriteDbContext { get; }

    public Task<Result<int>> Delete(Guid id, bool persist = true) => throw new NotImplementedException();

    public async Task<Result<int>> Insert(TModel model, bool persist = true)
        => await this.ManipulateModel(model, this.OnInserting, persist);

    async Task<Result<int>> IRepository<TModel>.SaveChangesAsync() =>
        //var result = await this.WriteDbContext.SaveChangesAsync();
        //return Result.Create(result, result > 0);
        await Task.FromResult(Result<int>.Succeed);

    public async Task<Result<int>> Update(TModel model, bool persist = true)
        => await this.ManipulateModel(model, this.OnUpdating, persist);

    protected virtual async Task<Result> OnInserting(TModel model)
    {
        _ = await this.WriteDbContext.AddAsync(model!);
        return Result.Succeed;
    }

    protected virtual Task<Result> OnUpdating(TModel model)
    {
        _ = this.WriteDbContext.Update(model!);
        return Task.FromResult(Result.Succeed);
    }

    protected virtual Result OnValidating(TModel? model)
        => Result.Succeed;

    private async Task<Result<int>> ManipulateModel(TModel model, Func<TModel, Task<Result>> action, bool persist = true)
    {
        var vr = this.OnValidating(model);
        if (vr?.IsSucceed != true)
        {
            return vr == null ? Result<int>.Failed : Result.Create<int>(default, vr);
        }

        var ir = await action(model);
        if (ir?.IsSucceed != true)
        {
            return ir == null ? Result<int>.Failed : Result.Create<int>(default, ir);
        }
        if (persist)
        {
            return await this.SaveChangesAsync();
        };
        return Result<int>.Succeed;
    }

    private async Task<Result<int>> SaveChangesAsync()
    {
        var sr = await this.WriteDbContext.SaveChangesAsync();
        return Result.Create(sr, sr > 0);
    }
}