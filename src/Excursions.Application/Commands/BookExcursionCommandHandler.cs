using System.Data;
using Excursions.Application.Responses;
using Excursions.Domain.Aggregates;
using Excursions.Domain.Exceptions;
using MediatR;

namespace Excursions.Application.Commands;

public record BookExcursionCommand(int Id, string TouristId) : IRequest<OperationResponse>;

public class BookExcursionCommandHandler : IRequestHandler<BookExcursionCommand, OperationResponse>
{
    private readonly IDataExecutionContext _dataExecutionContext;

    public BookExcursionCommandHandler(IDataExecutionContext dataExecutionContext)
    {
        _dataExecutionContext = dataExecutionContext;
    }
    
    public async Task<OperationResponse> Handle(BookExcursionCommand command, CancellationToken cancellationToken)
    {
        return await _dataExecutionContext.ExecuteWithTransactionAsync(
            async repositories =>
            {
                var excursion = await repositories.Excursion.GetByIdAsync(command.Id, cancellationToken);
                if (excursion is null)
                    throw new InvalidRequestException($"Excursion by {command.Id} id not found.");
                
                var booking = excursion.Book(command.TouristId);
                if (excursion.IsFree())
                    booking.Approve();

                await repositories.Excursion.UpdateAsync(excursion, cancellationToken);

                return new OperationResponse { IsSuccess = true };
            },
            IsolationLevel.Serializable,
            cancellationToken);
    }
}