using System.Security.Claims;
using Excursions.Api.Gql.Infrastructure;
using Excursions.Api.Infrastructure;
using Excursions.Application.Commands;
using Excursions.Application.Requests;
using Excursions.Application.Responses;
using MediatR;

namespace Excursions.Api.Gql.Schema.Mutations;

public class ExcursionMutation
{
    [GraphQLName("create")]
    public async Task<IdResponse> CreateAsync(
        [GqlClaimsPrincipal] ClaimsPrincipal claimsPrincipal,
        [Service] IMediator mediator,
        CreateExcursionRequest request)
    {
        var guideId = claimsPrincipal.GetUserId();
        var command = new CreateExcursionCommand(
            request.Name,
            request.Description,
            request.DateTimeUtc,
            request.PlacesCount,
            request.PricePerPlace,
            guideId);
        var response = await mediator.Send(command);
        return response;
    }
    
    [GraphQLName("update")]
    public async Task<OperationResponse> UpdateAsync(
        [GqlClaimsPrincipal] ClaimsPrincipal claimsPrincipal,
        [Service] IMediator mediator,
        UpdateExcursionRequest request)
    {
        var guideId = claimsPrincipal.GetUserId();
        var command = new UpdateExcursionCommand(
            request.Id,
            request.Name,
            request.Description,
            request.DateTimeUtc,
            request.PlacesCount,
            request.PricePerPlace,
            guideId);
        var response = await mediator.Send(command);
        return response;
    }

    [GraphQLName("publish")]
    public async Task<OperationResponse> PublishAsync(
        [GqlClaimsPrincipal] ClaimsPrincipal claimsPrincipal,
        [Service] IMediator mediator,
        int id)
    {
        var guideId = claimsPrincipal.GetUserId();
        var command = new PublishExcursionCommand(id, guideId);
        var response = await mediator.Send(command);
        return response;
    }
    
    [GraphQLName("book")]
    public async Task<OperationResponse> BookAsync(
        [GqlClaimsPrincipal] ClaimsPrincipal claimsPrincipal,
        [Service] IMediator mediator,
        int id)
    {
        var touristId = claimsPrincipal.GetUserId();
        var command = new BookExcursionCommand(id, touristId);
        var response = await mediator.Send(command);
        return response;
    }
}