using System.Data;
using Excursions.Domain.Aggregates;
using Excursions.Domain.Exceptions;
using MediatR;

namespace Excursions.Application.Commands;

public record ApproveBookingCommand(int Id) : IRequest;

public class ApproveBookingCommandHandler : AsyncRequestHandler<ApproveBookingCommand>
{
    private readonly IDataExecutionContext _dataExecutionContext;

    public ApproveBookingCommandHandler(IDataExecutionContext dataExecutionContext)
    {
        _dataExecutionContext = dataExecutionContext;
    }
    
    protected override async Task Handle(ApproveBookingCommand command, CancellationToken cancellationToken)
    {
        await _dataExecutionContext.ExecuteWithTransactionAsync(
            async repositories =>
            {
                var booking = await repositories.Booking.GetByIdAsync(command.Id, cancellationToken);
                if (booking is null)
                    throw new InvalidRequestException($"Booking by {command.Id} id not found.");
                
                booking.Approve();

                await repositories.Booking.UpdateAsync(booking, cancellationToken);
            },
            IsolationLevel.Snapshot,
            cancellationToken);
    }
}