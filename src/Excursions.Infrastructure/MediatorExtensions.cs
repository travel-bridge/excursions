using Excursions.Domain.Aggregates;
using MediatR;

namespace Excursions.Infrastructure;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEventsAsync(
        this IMediator mediator,
        DataContext context,
        CancellationToken cancellationToken)
    {
        var domainEntities = context.ChangeTracker
            .Entries<IWithDomainEvents>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var @event in domainEvents)
            await mediator.Publish(@event, cancellationToken);
    }
}