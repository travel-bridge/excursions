using System.Data;
using Excursions.Application.Responses;
using Excursions.Domain.Aggregates;
using Excursions.Domain.Exceptions;
using MediatR;

namespace Excursions.Application.Commands;

public record UpdateExcursionCommand(
    int Id,
    string? Name,
    string? Description,
    DateTime? DateTimeUtc,
    int? PlacesCount,
    decimal? PricePerPlace,
    string GuideId) : IRequest<OperationResponse>;

public class UpdateExcursionCommandHandler : IRequestHandler<UpdateExcursionCommand, OperationResponse>
{
    private readonly IDataExecutionContext _dataExecutionContext;

    public UpdateExcursionCommandHandler(IDataExecutionContext dataExecutionContext)
    {
        _dataExecutionContext = dataExecutionContext;
    }

    public async Task<OperationResponse> Handle(UpdateExcursionCommand command, CancellationToken cancellationToken)
    {
        return await _dataExecutionContext.ExecuteWithTransactionAsync(
            async repositories =>
            {
                var excursion = await repositories.Excursion.GetByIdAsync(command.Id, cancellationToken);
                if (excursion is null)
                    throw new InvalidRequestException($"Excursion by {command.Id} id not found.");
                
                if (excursion.GuideId != command.GuideId)
                    throw new AccessDeniedException($"Excursion access denied for guide with {command.GuideId} id.");

                excursion.Update(
                    command.Name,
                    command.Description,
                    command.DateTimeUtc,
                    command.PlacesCount,
                    command.PricePerPlace);

                await repositories.Excursion.UpdateAsync(excursion, cancellationToken);

                return new OperationResponse { IsSuccess = true };
            },
            IsolationLevel.Snapshot,
            cancellationToken);
    }
}