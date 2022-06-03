namespace Excursions.Domain.Aggregates;

public interface IRepository<TEntity, in TId>
    where TEntity : EntityBase<TId>, IAggregateRoot
    where TId : IEquatable<TId>
{
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
}