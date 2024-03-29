using System.Security.Claims;
using Excursions.Api.Gql.Infrastructure;
using Excursions.Api.Gql.Schema.Requests;
using Excursions.Api.Infrastructure;
using Excursions.Application.Commands;
using Excursions.Application.Responses;
using MediatR;

namespace Excursions.Api.Gql.Schema.Mutations;

public class ExcursionMutation
{
    [GraphQLName("create")]
    public async Task<CreateExcursionResponse> CreateAsync(
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
            new Domain.Aggregates.Optional<string?>(request.Description, request.Description.HasValue),
            request.DateTimeUtc,
            request.PlacesCount,
            new Domain.Aggregates.Optional<decimal?>(request.PricePerPlace, request.PricePerPlace.HasValue),
        guideId);
        await mediator.Send(command);

        return OperationResponse.Success;
    }

    [GraphQLName("publish")]
    public async Task<OperationResponse> PublishAsync(
        [GqlClaimsPrincipal] ClaimsPrincipal claimsPrincipal,
        [Service] IMediator mediator,
        int id)
    {
        var guideId = claimsPrincipal.GetUserId();
        var command = new PublishExcursionCommand(id, guideId);
        await mediator.Send(command);
        
        return OperationResponse.Success;
    }
    
    [GraphQLName("book")]
    public async Task<OperationResponse> BookAsync(
        [GqlClaimsPrincipal] ClaimsPrincipal claimsPrincipal,
        [Service] IMediator mediator,
        int id)
    {
        var touristId = claimsPrincipal.GetUserId();
        var command = new BookExcursionCommand(id, touristId);
        await mediator.Send(command);
        
        return OperationResponse.Success;
    }
}