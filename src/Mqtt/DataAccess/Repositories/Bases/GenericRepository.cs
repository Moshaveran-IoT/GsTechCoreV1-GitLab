using Moshaveran.Library.Data;
using Moshaveran.Library.Helpers;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

namespace Moshaveran.GsTech.Mqtt.DataAccess.Repositories.Bases;

internal class GenericRepository<TModel>(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : IRepository<TModel>
{
    protected MqttReadDbContext ReadDbContext { get; } = readDbContext;
    protected MqttWriteDbContext WriteDbContext { get; } = writeDbContext;

    public Task<IResult> Delete(TModel model, bool persist = true, CancellationToken token = default)
        => this.ManipulateModel(model, this.OnDeleting, persist, token);

    public Task<IResult> Insert(TModel model, bool persist = true, CancellationToken token = default)
            => this.ManipulateModel(model, this.OnInserting, persist, token);

    public async Task<IResult> SaveChanges(CancellationToken token = default)
    {
        try
        {
            return await this.OnSavingChanges(token);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }

    public Task<IResult> Update(TModel model, bool persist = true, CancellationToken token = default)
            => this.ManipulateModel(model, this.OnUpdating, persist, token);

    protected virtual async Task<IResult> ManipulateModel(TModel model, Func<TModel, CancellationToken, Task<IResult>> action, bool persist = true, CancellationToken token = default)
    {
        try
        {
            var vr = this.OnValidating(model, token);
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
                return await this.SaveChanges(token);
            };
            return Result.Succeed;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }

    protected virtual Task<IResult> OnDeleting(TModel model, CancellationToken token)
    {
        _ = this.WriteDbContext.Remove(model!);
        return Result.Succeed.ToAsync();
    }

    protected virtual async Task<IResult> OnInserting(TModel model, CancellationToken token = default)
    {
        _ = await this.WriteDbContext.AddAsync(model!, token);
        return Result.Succeed;
    }

    protected virtual async Task<IResult> OnSavingChanges(CancellationToken token)
    {
        var result = await this.WriteDbContext.SaveChangesAsync(token);
        return Result.Create(result > 0);
    }

    protected virtual Task<IResult> OnUpdating(TModel model, CancellationToken token = default)
    {
        _ = this.WriteDbContext.Update(model!);
        return Result.Succeed.ToAsync();
    }

    protected virtual IResult OnValidating(TModel? model, CancellationToken token = default)
        => Result.Succeed;
}