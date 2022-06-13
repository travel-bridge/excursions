using System.Data;
using Excursions.Application.Events;
using Excursions.Domain.Aggregates;
using Excursions.Domain.Exceptions;
using MediatR;

namespace Excursions.Application.Commands;

public record BookExcursionCommand(int Id, string TouristId) : IRequest;

public class BookExcursionCommandHandler : AsyncRequestHandler<BookExcursionCommand>
{
    private readonly IDataExecutionContext _dataExecutionContext;
    private readonly IEventProducer _eventProducer;

    public BookExcursionCommandHandler(IDataExecutionContext dataExecutionContext, IEventProducer eventProducer)
    {
        _dataExecutionContext = dataExecutionContext;
        _eventProducer = eventProducer;
    }
    
    protected override async Task Handle(BookExcursionCommand command, CancellationToken cancellationToken)
    {
        await _dataExecutionContext.ExecuteWithTransactionAsync(
            async repositories =>
            {
                var excursion = await repositories.Excursion.GetByIdAsync(command.Id, cancellationToken);
                if (excursion is null)
                    throw new InvalidRequestException($"Excursion by {command.Id} id not found.");
                
                var booking = excursion.Book(command.TouristId);
                if (excursion.IsFree())
                    booking.Approve();

                await repositories.Excursion.UpdateAsync(excursion, cancellationToken);

                if (!excursion.IsFree())
                    await _eventProducer.ProduceAsync(
                        new PaidExcursionBookedEvent(booking.Id, booking.TouristId),
                        cancellationToken);
            },
            IsolationLevel.Serializable,
            cancellationToken);
    }
}