using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

namespace Moshaveran.Mqtt.DataAccess.Repositories.Bases;

internal class GenericRepository<TModel> : IRepository<TModel>
{
    public GenericRepository(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext)
        => (this.WriteDbContext, this.ReadDbContext) = (writeDbContext, readDbContext);

    protected MqttReadDbContext ReadDbContext { get; }
    protected MqttWriteDbContext WriteDbContext { get; }

    public Task<Result> Delete(TModel model, bool persist = true, CancellationToken token = default)
        => this.ManipulateModel(model, this.OnDeleting, persist, token);

    public Task<Result> Insert(TModel model, bool persist = true, CancellationToken token = default)
        => this.ManipulateModel(model, this.OnInserting, persist, token);

    public Task<Result> SaveChanges(CancellationToken token = default)
        => this.OnSavingChanges(token);

    public Task<Result> Update(TModel model, bool persist = true, CancellationToken token = default)
        => this.ManipulateModel(model, this.OnUpdating, persist, token);

    protected virtual async Task<Result> ManipulateModel(TModel model, Func<TModel, CancellationToken, Task<Result>> action, bool persist = true, CancellationToken token = default)
    {
        try
        {
            var vr = OnValidating(model, token);
            if (vr?.IsSucceed != true)
            {
                return vr ?? Result.Failed;
            }

            var ar = await action(model, token);
            if (ar?.IsSucceed != true)
            {
                return ar ?? Result.Failed;
            }

            if (persist)
            {
                return await SaveChanges(token);
            };
            return Result.Succeed;
        }
        catch (Exception ex)
        {
            return Result.CreateFailure(ex);
        }
    }

    protected virtual Task<Result> OnDeleting(TModel model, CancellationToken token)
    {
        _ = WriteDbContext.Remove(model!);
        return Task.FromResult(Result.Succeed);
    }

    protected virtual async Task<Result> OnInserting(TModel model, CancellationToken token = default)
    {
        _ = await WriteDbContext.AddAsync(model!, token);
        return Result.Succeed;
    }

    protected virtual async Task<Result> OnSavingChanges(CancellationToken token)
    {
        var result = await WriteDbContext.SaveChangesAsync(token);
        return Result.Create(result > 0);
    }

    protected virtual Task<Result> OnUpdating(TModel model, CancellationToken token = default)
    {
        _ = WriteDbContext.Update(model!);
        return Task.FromResult(Result.Succeed);
    }

    protected virtual Result OnValidating(TModel? model, CancellationToken token = default)
        => Result.Succeed;
}