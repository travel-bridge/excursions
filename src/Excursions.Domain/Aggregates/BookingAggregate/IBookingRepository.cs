namespace Excursions.Domain.Aggregates.BookingAggregate;

public interface IBookingRepository : IRepository<Booking, int>
{
    Task<Booking?> GetFirstExpiredAsync(DateTime expirationDateTimeUtc);
}