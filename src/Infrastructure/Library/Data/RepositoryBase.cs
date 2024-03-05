using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Moshaveran.Library.Helpers;
using Moshaveran.Library.Results;

using System.Diagnostics.CodeAnalysis;

namespace Moshaveran.Library.Data;

public abstract class RepositoryBase<TModel, TReadDbContext, TWriteDbContext>(in TReadDbContext readDbContext, in TWriteDbContext writeDbContext, in ILogger logger) : IRepository<TModel>
    where TModel : class
    where TReadDbContext : DbContext
    where TWriteDbContext : DbContext
{
    public ILogger Logger { get; } = logger;
    protected TReadDbContext ReadDbContext { get; } = readDbContext;
    protected TWriteDbContext WriteDbContext { get; } = writeDbContext;

    public Task<IResult> Delete(TModel model, bool persist = true, CancellationToken cancellationToken = default)
        => this.ManipulateModel(model, this.OnDeleting, persist, cancellationToken: cancellationToken);

    public async Task<IList<TModel>> GetAll(CancellationToken cancellationToken = default)
    {
        this.Logger.LogTrace($"[{nameof(TModel)}] - {nameof(GetAll)}");
        var query = this.ReadDbContext.Set<TModel>();
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<TModel?> GetById(int id, CancellationToken cancellationToken = default)
    {
        this.Logger.LogTrace($"[{nameof(TModel)}] - {nameof(GetAll)}");
        var query = this.ReadDbContext.Set<TModel>();
        return await query.FindAsync([id], cancellationToken: cancellationToken);
    }

    public Task<IResult> Insert(TModel model, bool persist = true, CancellationToken cancellationToken = default)
    {
        this.Logger.LogTrace($"[{nameof(TModel)}] - {nameof(Insert)}");
        return this.ManipulateModel(model, this.OnInserting, persist, cancellationToken: cancellationToken);
    }

    public async Task<IResult> SaveChanges(CancellationToken cancellationToken = default)
    {
        this.Logger.LogTrace($"[{nameof(TModel)}] - {nameof(SaveChanges)}");
        try
        {
            var result = await this.OnSavingChanges(cancellationToken);
            this.Logger.LogTrace($"[{nameof(TModel)}] - {nameof(SaveChanges)} - [Done]");
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            this.Logger.LogTrace($"[{nameof(TModel)}] - {nameof(SaveChanges)} - [Handled Exception]: {ex.GetBaseException().Message}");
            return IResult.Fail(ex);
        }
        catch (Exception ex)
        {
            this.Logger.LogError("[{nameof(TModel)}] - {nameof(SaveChanges)} - [Unhandled Exception]: {ex.GetBaseException().Message}"
                , nameof(TModel), nameof(SaveChanges), ex.GetBaseException().Message);
            throw;
        }
    }

    Task<IResult> IRepository<TModel>.SaveChanges(CancellationToken cancellationToken) => throw new NotImplementedException();

    public Task<IResult> Update(TModel model, bool persist = true, CancellationToken cancellationToken = default)
    {
        this.Logger.LogTrace($"[{nameof(TModel)}] - {nameof(Update)}");
        return this.ManipulateModel(model, this.OnUpdating, persist, cancellationToken: cancellationToken);
    }

    protected virtual async Task<IResult> ManipulateModel(TModel model, Func<TModel, CancellationToken, Task<IResult>> action, bool persist = true, bool shouldValidate = true, CancellationToken cancellationToken = default)
    {
        try
        {
            if (shouldValidate)
            {
                var vr = await this.OnValidating(model, cancellationToken);
                if (!vr.IsSucceed())
                {
                    return vr ?? IResult.Failed;
                }
            }

            var ar = await action(model, cancellationToken);
            if (!ar.IsSucceed())
            {
                return ar ?? IResult.Failed;
            }

            if (persist)
            {
                return await this.SaveChanges(cancellationToken);
            };
            return IResult.Succeed;
        }
        catch (DbUpdateConcurrencyException exception)
        {
            return IResult.Fail(exception);
        }
    }

    protected virtual Task<IResult> OnDeleting([DisallowNull] TModel model, CancellationToken cancellationToken = default)
    {
        _ = this.WriteDbContext.Remove(model);
        return IResult.Success().ToAsync();
    }

    protected virtual async Task<IResult> OnInserting([DisallowNull] TModel model, CancellationToken cancellationToken = default)
    {
        _ = await this.WriteDbContext.AddAsync(model, cancellationToken);
        return IResult.Success();
    }

    protected virtual async Task<IResult> OnSavingChanges(CancellationToken cancellationToken = default)
    {
        var sr = await this.WriteDbContext.SaveChangesAsync(cancellationToken);
        return IResult.Create(sr > 0);
    }

    protected virtual Task<IResult> OnUpdating([DisallowNull] TModel model, CancellationToken cancellationToken = default)
    {
        _ = this.WriteDbContext.Update(model);
        return IResult.Success().ToAsync();
    }

    protected virtual Task<IResult> OnValidating([DisallowNull] TModel? model, CancellationToken cancellationToken = default)
        => IResult.Succeed.ToAsync();
}