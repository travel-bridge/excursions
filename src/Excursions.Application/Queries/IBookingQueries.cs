using Excursions.Application.Responses;

namespace Excursions.Application.Queries;

public interface IBookingQueries
{
    Task<PageableResponse<BookingResponse>> GetByTouristAsync(
        string touristId,
        int skip = 0,
        int take = 20);
}