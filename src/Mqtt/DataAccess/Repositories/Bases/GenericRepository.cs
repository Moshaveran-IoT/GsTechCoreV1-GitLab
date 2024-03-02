using Microsoft.EntityFrameworkCore;

using Moshaveran.Library.Data;
using Moshaveran.Library.Helpers;
using Moshaveran.Mqtt.DataAccess.DataSources.DbContexts;

namespace Moshaveran.GsTech.Mqtt.DataAccess.Repositories.Bases;

internal class GenericRepository<TModel>(MqttReadDbContext readDbContext, MqttWriteDbContext writeDbContext) : IRepository<TModel>
    where TModel : class
{
    protected MqttReadDbContext ReadDbContext { get; } = readDbContext;
    protected MqttWriteDbContext WriteDbContext { get; } = writeDbContext;

    public ValueTask<IResult> Delete(TModel model, bool persist = true, CancellationToken cancellationToken = default)
        => this.ManipulateModel(model, this.OnDeleting, persist, cancellationToken: cancellationToken);

    public async ValueTask<IList<TModel>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var query = this.ReadDbContext.Set<TModel>();
        return await query.ToListAsync(cancellationToken);
    }

    public async ValueTask<TModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var query = this.ReadDbContext.Set<TModel>();
        return await query.FindAsync([id], cancellationToken: cancellationToken);
    }

    public ValueTask<IResult> Insert(TModel model, bool persist = true, CancellationToken cancellationToken = default)
        => this.ManipulateModel(model, this.OnInserting, persist, cancellationToken: cancellationToken);

    public async ValueTask<IResult> SaveChanges(CancellationToken cancellationToken = default)
    {
        try
        {
            return await this.OnSavingChanges(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }

    public ValueTask<IResult> Update(TModel model, bool persist = true, CancellationToken cancellationToken = default)
        => this.ManipulateModel(model, this.OnUpdating, persist, cancellationToken: cancellationToken);

    protected virtual async ValueTask<IResult> ManipulateModel(TModel model, Func<TModel, CancellationToken, ValueTask<IResult>> action, bool persist = true, bool shouldValidate = true, CancellationToken cancellationToken = default)
    {
        try
        {
            if (shouldValidate)
            {
                var vr = this.OnValidating(model, cancellationToken);
                if (vr?.IsSucceed != true)
                {
                    return vr ?? Result.Failed;
                }
            }

            var ar = await action(model, cancellationToken);
            if (ar?.IsSucceed != true)
            {
                return ar ?? Result.Failed;
            }

            if (persist)
            {
                return await this.SaveChanges(cancellationToken);
            };
            return Result.Succeed;
        }
        catch (Exception ex)
        {
            return Result.Fail(ex);
        }
    }

    protected virtual ValueTask<IResult> OnDeleting(TModel model, CancellationToken cancellationToken)
    {
        _ = this.WriteDbContext.Remove(model!);
        return Result.Succeed.ToAsync();
    }

    protected virtual async ValueTask<IResult> OnInserting(TModel model, CancellationToken cancellationToken = default)
    {
        _ = await this.WriteDbContext.AddAsync(model!, cancellationToken);
        return Result.Succeed;
    }

    protected virtual async ValueTask<IResult> OnSavingChanges(CancellationToken cancellationToken)
    {
        var result = await this.WriteDbContext.SaveChangesAsync(cancellationToken);
        return Result.Create(result > 0);
    }

    protected virtual ValueTask<IResult> OnUpdating(TModel model, CancellationToken cancellationToken = default)
    {
        _ = this.WriteDbContext.Update(model!);
        return Result.Succeed.ToAsync();
    }

    protected virtual IResult OnValidating(TModel? model, CancellationToken cancellationToken = default)
        => Result.Succeed;
}