using Excursions.Domain.Aggregates;
using Excursions.Domain.Aggregates.ExcursionAggregate;
using Excursions.Infrastructure.Repositories;

namespace Excursions.Infrastructure;

public class RepositoryRegistry : IRepositoryRegistry
{
    public RepositoryRegistry(DataContext dataContext)
    {
        Excursion = new ExcursionRepository(dataContext);
    }
    
    public IExcursionRepository Excursion { get; }
}