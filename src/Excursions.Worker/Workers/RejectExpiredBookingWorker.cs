using Excursions.Application.Commands;
using Excursions.Application.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Excursions.Worker.Workers;

public class RejectExpiredBookingWorker : WorkerBase
{
    private const int WorkerDelayMilliseconds = 300000;
    
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RejectExpiredBookingWorker(
        IServiceScopeFactory serviceScopeFactory,
        IStringLocalizer<ExcursionResource> stringLocalizer,
        ILogger<RejectExpiredBookingWorker> logger)
        : base(stringLocalizer, logger)
    {
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
                    bool isRejected;
                    do
                    {
                        var command = new RejectFirstExpiredBookingCommand();
                        isRejected = await mediator.Send(command, stoppingToken);
                    }
                    while (isRejected);
                },
                stoppingToken);

            await ExecuteSafelyAsync(
                async () => await Task.Delay(WorkerDelayMilliseconds, stoppingToken),
                stoppingToken);
        }
        while (!stoppingToken.IsCancellationRequested);
    }
}