using Excursions.Application.Commands;
using Excursions.Application.Queries;
using Excursions.Application.Resources;
using Excursions.Domain.Aggregates.ExcursionAggregate;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Excursions.Worker.Workers;

public class BookingRejectionWorker : WorkerBase
{
    private const int WorkerDelayMilliseconds = 300000;
    private const double BookingApproveLimitMinutes = 10;

    private readonly IBookingQueries _bookingQueries;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public BookingRejectionWorker(
        IBookingQueries bookingQueries,
        IServiceScopeFactory serviceScopeFactory,
        IStringLocalizer<ExcursionResource> stringLocalizer,
        ILogger<BookingRejectionWorker> logger)
        : base(stringLocalizer, logger)
    {
        _bookingQueries = bookingQueries;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        do
        {
            await ExecuteSafelyAsync(
                async () =>
                {
                    while (true)
                    {
                        var booking = await _bookingQueries.GetFirstToRejectAsync(
                            BookingStatus.Booked,
                            DateTime.UtcNow.AddMinutes(-BookingApproveLimitMinutes));

                        if (booking == null)
                            break;
                        
                        var command = new RejectBookingCommand(booking.ExcursionId, booking.TouristId);
                        await mediator.Send(command, stoppingToken);
                    }
                },
                stoppingToken);

            await Task.Delay(WorkerDelayMilliseconds, stoppingToken);
        }
        while (!stoppingToken.IsCancellationRequested);
    }
}