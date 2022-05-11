using System.Data;
using Excursions.Application.Responses;
using Excursions.Domain.Aggregates;
using Excursions.Domain.Exceptions;
using MediatR;

namespace Excursions.Application.Commands;

public record ApproveBookingCommand(int Id) : IRequest<OperationResponse>;

public class ApproveBookingCommandHandler : IRequestHandler<ApproveBookingCommand, OperationResponse>
{
    private readonly IDataExecutionContext _dataExecutionContext;

    public ApproveBookingCommandHandler(IDataExecutionContext dataExecutionContext)
    {
        _dataExecutionContext = dataExecutionContext;
    }
    
    public async Task<OperationResponse> Handle(ApproveBookingCommand command, CancellationToken cancellationToken)
    {
        return await _dataExecutionContext.ExecuteWithTransactionAsync(
            async repositories =>
            {
                var booking = await repositories.Booking.GetByIdAsync(command.Id, cancellationToken);
                if (booking is null)
                    throw new InvalidRequestException($"Booking by {command.Id} id not found.");
                
                booking.Approve();

                await repositories.Booking.UpdateAsync(booking, cancellationToken);

                return new OperationResponse { IsSuccess = true };
            },
            IsolationLevel.Snapshot,
            cancellationToken);
    }
}