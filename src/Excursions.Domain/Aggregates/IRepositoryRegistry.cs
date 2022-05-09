using Excursions.Domain.Aggregates.ExcursionAggregate;

namespace Excursions.Domain.Aggregates;

public interface IRepositoryRegistry
{
    IExcursionRepository Excursion { get; }
}