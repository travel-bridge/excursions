using System.Data;
using Excursions.Domain.Aggregates;
using Excursions.Domain.Exceptions;
using MediatR;

namespace Excursions.Application.Commands;

public record UpdateExcursionCommand(
    int Id,
    string? Name,
    string? Description,
    bool IsDescriptionHasValue,
    DateTime? DateTimeUtc,
    int? PlacesCount,
    decimal? PricePerPlace,
    bool IsPricePerPlaceHasValue,
    string GuideId) : IRequest;

public class UpdateExcursionCommandHandler : AsyncRequestHandler<UpdateExcursionCommand>
{
    private readonly IDataExecutionContext _dataExecutionContext;

    public UpdateExcursionCommandHandler(IDataExecutionContext dataExecutionContext)
    {
        _dataExecutionContext = dataExecutionContext;
    }

    protected override async Task Handle(UpdateExcursionCommand command, CancellationToken cancellationToken)
    {
        await _dataExecutionContext.ExecuteWithTransactionAsync(
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
                    command.IsDescriptionHasValue,
                    command.DateTimeUtc,
                    command.PlacesCount,
                    command.PricePerPlace,
                    command.IsPricePerPlaceHasValue);

                await repositories.Excursion.UpdateAsync(excursion, cancellationToken);
            },
            IsolationLevel.Snapshot,
            cancellationToken);
    }
}