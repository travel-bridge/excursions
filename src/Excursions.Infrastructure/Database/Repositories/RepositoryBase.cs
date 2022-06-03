using Excursions.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Excursions.Infrastructure.Database.Repositories;

public abstract class RepositoryBase<TDbContext, TEntity, TId> : IRepository<TEntity, TId>
    where TDbContext : DbContext
    where TEntity : EntityBase<TId>, IAggregateRoot
    where TId : IEquatable<TId>
{
    protected RepositoryBase(TDbContext context)
    {
        Context = context;
        Set = Context.Set<TEntity>();
        QuerySet = Context.Set<TEntity>();
    }
    
    protected TDbContext Context { get; }

    protected DbSet<TEntity> Set { get; }
    
    protected virtual IQueryable<TEntity> QuerySet { get; }

    public async Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Set.Add(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Set.Update(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Set.Remove(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        var entity = await Set.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (entity is not null)
            await DeleteAsync(entity, cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default) =>
        await QuerySet.FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
}