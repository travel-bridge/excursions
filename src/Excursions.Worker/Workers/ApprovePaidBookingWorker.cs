using Excursions.Application.Commands;
using Excursions.Application.IntegrationEvents;
using Excursions.Application.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Excursions.Worker.Workers;

public class ApprovePaidBookingWorker : WorkerBase
{
    private const string ConsumerGroupId = "approve-paid-booking-consumer-group";

    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IEventConsumerFactory _eventConsumerFactory;
    
    public ApprovePaidBookingWorker(
        IServiceScopeFactory serviceScopeFactory,
        IEventConsumerFactory eventConsumerFactory,
        IStringLocalizer<ExcursionResource> stringLocalizer,
        ILogger<WorkerBase> logger)
        : base(stringLocalizer, logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _eventConsumerFactory = eventConsumerFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        using var eventConsumer = _eventConsumerFactory.Subscribe(
            Topics.PaidExcursionBookingApproved,
            ConsumerGroupId);

        do
        {
            await ExecuteSafelyAsync(
                async () =>
                {
                    await eventConsumer.ConsumeAndHandleAsync<PaidExcursionBookingApprovedIntegrationEvent>(
                        async @event => await mediator.Send(
                            new ApproveBookingCommand(@event.BookingId),
                            stoppingToken),
                        stoppingToken);
                },
                stoppingToken);
        }
        while (!stoppingToken.IsCancellationRequested);
    }
}