using System.Data;
using Excursions.Application.Responses;
using Excursions.Domain.Aggregates;
using Excursions.Domain.Exceptions;
using MediatR;

namespace Excursions.Application.Commands;

public record RejectBookingCommand(int ExcursionId, string TouristId) : IRequest<OperationResponse>;

public class RejectBookingCommandHandler : IRequestHandler<RejectBookingCommand, OperationResponse>
{
    private readonly IDataExecutionContext _dataExecutionContext;

    public RejectBookingCommandHandler(IDataExecutionContext dataExecutionContext)
    {
        _dataExecutionContext = dataExecutionContext;
    }
    
    public async Task<OperationResponse> Handle(RejectBookingCommand command, CancellationToken cancellationToken)
    {
        return await _dataExecutionContext.ExecuteWithTransactionAsync(
            async repositories =>
            {
                var excursion = await repositories.Excursion.GetByIdAsync(command.ExcursionId, cancellationToken);
                if (excursion is null)
                    throw new InvalidRequestException($"Excursion by {command.ExcursionId} id not found.");
                
                excursion.RejectBooking(command.TouristId);

                await repositories.Excursion.UpdateAsync(excursion, cancellationToken);

                return new OperationResponse { IsSuccess = true };
            },
            IsolationLevel.Snapshot,
            cancellationToken);
    }
}