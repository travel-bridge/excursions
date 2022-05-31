using Excursions.Domain.Aggregates.BookingAggregate;
using Microsoft.EntityFrameworkCore;

namespace Excursions.Infrastructure.Database.Repositories;

public class BookingRepository : RepositoryBase<DataContext, Booking, int>, IBookingRepository
{
    public BookingRepository(DataContext context) : base(context)
    {
    }

    public async Task<Booking?> GetFirstExpiredAsync(DateTime expirationDateTimeUtc) =>
        await QuerySet.FirstOrDefaultAsync(x => x.CreateDateTimeUtc <= expirationDateTimeUtc
            && x.Status == BookingStatus.Booked);
}