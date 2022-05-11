using Excursions.Domain.Aggregates;
using Excursions.Domain.Aggregates.BookingAggregate;
using Excursions.Domain.Aggregates.ExcursionAggregate;
using Excursions.Infrastructure.Repositories;

namespace Excursions.Infrastructure;

public class RepositoryRegistry : IRepositoryRegistry
{
    public RepositoryRegistry(DataContext dataContext)
    {
        Excursion = new ExcursionRepository(dataContext);
        Booking = new BookingRepository(dataContext);
    }
    
    public IExcursionRepository Excursion { get; }
    
    public IBookingRepository Booking { get; }
}