using Excursions.Domain.Aggregates.BookingAggregate;
using Excursions.Domain.Aggregates.ExcursionAggregate;

namespace Excursions.Domain.Aggregates;

public interface IRepositoryRegistry
{
    IExcursionRepository Excursion { get; }

    IBookingRepository Booking { get; }
}