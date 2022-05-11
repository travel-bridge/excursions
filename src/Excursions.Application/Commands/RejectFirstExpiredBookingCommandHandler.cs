using System.Data;
using Excursions.Application.Responses;
using Excursions.Domain.Aggregates;
using MediatR;

namespace Excursions.Application.Commands;

public record RejectFirstExpiredBookingCommand : IRequest<OperationResponse>;

public class RejectFirstExpiredBookingCommandHandler
    : IRequestHandler<RejectFirstExpiredBookingCommand, OperationResponse>
{
    private const double BookingExpirationMinutes = 10;
    
    private readonly IDataExecutionContext _dataExecutionContext;

    public RejectFirstExpiredBookingCommandHandler(IDataExecutionContext dataExecutionContext)
    {
        _dataExecutionContext = dataExecutionContext;
    }
    
    public async Task<OperationResponse> Handle(
        RejectFirstExpiredBookingCommand command,
        CancellationToken cancellationToken)
    {
        return await _dataExecutionContext.ExecuteWithTransactionAsync(
            async repositories =>
            {
                var expirationDateTimeUtc = DateTime.UtcNow.AddMinutes(-BookingExpirationMinutes);
                var booking = await repositories.Booking.GetFirstExpiredAsync(expirationDateTimeUtc);
                if (booking == null)
                    return new OperationResponse { IsSuccess = false };
                
                booking.Reject();

                await repositories.Booking.UpdateAsync(booking, cancellationToken);

                return new OperationResponse { IsSuccess = true };
            },
            IsolationLevel.Serializable,
            cancellationToken);
    }
}