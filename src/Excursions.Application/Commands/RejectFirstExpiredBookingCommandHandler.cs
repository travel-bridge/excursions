using System.Data;
using Excursions.Domain.Aggregates;
using MediatR;

namespace Excursions.Application.Commands;

public record RejectFirstExpiredBookingCommand : IRequest<bool>;

public class RejectFirstExpiredBookingCommandHandler : IRequestHandler<RejectFirstExpiredBookingCommand, bool>
{
    private const double BookingExpirationMinutes = 10;
    
    private readonly IDataExecutionContext _dataExecutionContext;

    public RejectFirstExpiredBookingCommandHandler(IDataExecutionContext dataExecutionContext)
    {
        _dataExecutionContext = dataExecutionContext;
    }
    
    public async Task<bool> Handle(
        RejectFirstExpiredBookingCommand command,
        CancellationToken cancellationToken)
    {
        return await _dataExecutionContext.ExecuteWithTransactionAsync(
            async repositories =>
            {
                var expirationDateTimeUtc = DateTime.UtcNow.AddMinutes(-BookingExpirationMinutes);
                var booking = await repositories.Booking.GetFirstExpiredAsync(expirationDateTimeUtc);
                if (booking == null)
                    return false;
                
                booking.Reject();

                await repositories.Booking.UpdateAsync(booking, cancellationToken);

                return true;
            },
            IsolationLevel.Serializable,
            cancellationToken);
    }
}