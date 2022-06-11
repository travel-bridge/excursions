using System.Data;
using Excursions.Domain.Aggregates;
using Excursions.Domain.Exceptions;
using MediatR;

namespace Excursions.Application.Commands;

public record PublishExcursionCommand(int Id, string GuideId) : IRequest;

public class PublishExcursionCommandHandler : AsyncRequestHandler<PublishExcursionCommand>
{
    private readonly IDataExecutionContext _dataExecutionContext;

    public PublishExcursionCommandHandler(IDataExecutionContext dataExecutionContext)
    {
        _dataExecutionContext = dataExecutionContext;
    }
    
    protected override async Task Handle(PublishExcursionCommand command, CancellationToken cancellationToken)
    {
        await _dataExecutionContext.ExecuteWithTransactionAsync(
            async repositories =>
            {
                var excursion = await repositories.Excursion.GetByIdAsync(command.Id, cancellationToken);
                if (excursion is null)
                    throw new InvalidRequestException($"Excursion by {command.Id} id not found.");
                
                if (excursion.GuideId != command.GuideId)
                    throw new AccessDeniedException($"Excursion access denied for guide with {command.GuideId} id.");
                
                excursion.Publish();

                await repositories.Excursion.UpdateAsync(excursion, cancellationToken);
            },
            IsolationLevel.Snapshot,
            cancellationToken);
    }
}