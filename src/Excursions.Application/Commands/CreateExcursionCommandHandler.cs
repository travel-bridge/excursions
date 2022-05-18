using System.Data;
using Excursions.Application.Responses;
using Excursions.Domain.Aggregates;
using Excursions.Domain.Aggregates.ExcursionAggregate;
using MediatR;

namespace Excursions.Application.Commands;

public record CreateExcursionCommand(
    string Name,
    string? Description,
    DateTime DateTimeUtc,
    int PlacesCount,
    decimal? PricePerPlace,
    string GuideId) : IRequest<IdResponse>;

public class CreateExcursionCommandHandler : IRequestHandler<CreateExcursionCommand, IdResponse>
{
    private readonly IDataExecutionContext _dataExecutionContext;

    public CreateExcursionCommandHandler(IDataExecutionContext dataExecutionContext)
    {
        _dataExecutionContext = dataExecutionContext;
    }
    
    public async Task<IdResponse> Handle(CreateExcursionCommand command, CancellationToken cancellationToken)
    {
        return await _dataExecutionContext.ExecuteWithTransactionAsync(
            async repositories =>
            {
                var excursion = Excursion.Create(
                    command.Name,
                    command.Description,
                    command.DateTimeUtc,
                    command.PlacesCount,
                    command.PricePerPlace,
                    command.GuideId);

                await repositories.Excursion.CreateAsync(excursion, cancellationToken);

                return new IdResponse { Id = excursion.Id };
            },
            IsolationLevel.Snapshot,
            cancellationToken);
    }
}