namespace Excursions.Domain.Aggregates.BookingAggregate;

public interface IBookingRepository : IRepository<Booking>
{
    Task<Booking?> GetFirstExpiredAsync(DateTime expirationDateTimeUtc);
}