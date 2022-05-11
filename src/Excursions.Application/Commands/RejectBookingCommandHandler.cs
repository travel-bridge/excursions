using System.Data;
using Excursions.Application.Responses;
using Excursions.Domain.Aggregates;
using Excursions.Domain.Exceptions;
using MediatR;

namespace Excursions.Application.Commands;

public record RejectBookingCommand(int Id) : IRequest<OperationResponse>;

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
                var booking = await repositories.Booking.GetByIdAsync(command.Id, cancellationToken);
                if (booking is null)
                    throw new InvalidRequestException($"Booking by {command.Id} id not found.");
                
                booking.Reject();

                await repositories.Booking.UpdateAsync(booking, cancellationToken);

                return new OperationResponse { IsSuccess = true };
            },
            IsolationLevel.Snapshot,
            cancellationToken);
    }
}