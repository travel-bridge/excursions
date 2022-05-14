using System.Security.Claims;
using Excursions.Api.Gql.Infrastructure;
using Excursions.Api.Infrastructure;
using Excursions.Application.Queries;
using Excursions.Application.Responses;

namespace Excursions.Api.Gql.Schema.Queries;

public class ExcursionQuery
{
    [GraphQLName("byId")]
    public async Task<ExcursionResponse?> GetByIdAsync(
        [Service] IExcursionQueries excursionQueries,
        int id)
    {
        var response = await excursionQueries.GetByIdAsync(id);
        return response;
    }

    [GraphQLName("search")]
    public async Task<PageableResponse<ExcursionResponse>> GetAsync(
        [Service] IExcursionQueries excursionQueries,
        int skip = 0,
        int take = 20)
    {
        var response = await excursionQueries.GetAsync(skip, take);
        return response;
    }

    [GraphQLName("searchForGuide")]
    public async Task<PageableResponse<ExcursionResponse>> GetByGuideIdAsync(
        [GqlClaimsPrincipal] ClaimsPrincipal claimsPrincipal,
        [Service] IExcursionQueries excursionQueries,
        int skip = 0,
        int take = 20)
    {
        var guideId = claimsPrincipal.GetUserId();
        var response = await excursionQueries.GetByGuideIdAsync(guideId, skip, take);
        return response;
    }
}