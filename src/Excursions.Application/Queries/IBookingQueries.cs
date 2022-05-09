using Excursions.Application.Responses;
using Excursions.Domain.Aggregates.ExcursionAggregate;

namespace Excursions.Application.Queries;

public interface IBookingQueries
{
    Task<PageableResponse<BookingResponse>> GetByTouristAsync(
        string touristId,
        int skip = 0,
        int take = 20);

    Task<BookingToRejectResponse?> GetFirstToRejectAsync(
        BookingStatus status,
        DateTime dateTimeUtc);
}