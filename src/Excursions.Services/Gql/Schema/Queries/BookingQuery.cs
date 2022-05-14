using System.Security.Claims;
using Excursions.Api.Gql.Infrastructure;
using Excursions.Api.Infrastructure;
using Excursions.Application.Queries;
using Excursions.Application.Responses;

namespace Excursions.Api.Gql.Schema.Queries;

public class BookingQuery
{
    [GraphQLName("searchForTourist")]
    public async Task<PageableResponse<BookingResponse>> GetByTouristAsync(
        [GqlClaimsPrincipal] ClaimsPrincipal claimsPrincipal,
        [Service] IBookingQueries bookingQueries,
        int skip = 0,
        int take = 20)
    {
        var touristId = claimsPrincipal.GetUserId();
        var response = await bookingQueries.GetByTouristAsync(touristId, skip, take);
        return response;
    }
}