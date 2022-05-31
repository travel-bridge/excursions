using System.Data;
using Excursions.Domain.Aggregates;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Excursions.Infrastructure.Database;

public class DataExecutionContext : IDataExecutionContext
{
    private readonly DataContext _dataContext;
    private readonly IRepositoryRegistry _repositoryRegistry;
    private readonly IMediator _mediator;

    public DataExecutionContext(
        DataContext dataContext,
        IRepositoryRegistry repositoryRegistry,
        IMediator mediator)
    {
        _dataContext = dataContext;
        _repositoryRegistry = repositoryRegistry;
        _mediator = mediator;
    }
    
    public async Task ExecuteWithTransactionAsync(
        Func<IRepositoryRegistry, Task> action,
        IsolationLevel isolationLevel = IsolationLevel.Snapshot,
        CancellationToken cancellationToken = default)
    {
        var executionStrategy = _dataContext.Database.CreateExecutionStrategy();

        await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dataContext.Database.BeginTransactionAsync(
                isolationLevel,
                cancellationToken);

            await action(_repositoryRegistry);
            await _mediator.DispatchDomainEventsAsync(_dataContext, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        });
    }

    public async Task<TResponse> ExecuteWithTransactionAsync<TResponse>(
        Func<IRepositoryRegistry, Task<TResponse>> func,
        IsolationLevel isolationLevel = IsolationLevel.Snapshot,
        CancellationToken cancellationToken = default)
    {
        var executionStrategy = _dataContext.Database.CreateExecutionStrategy();

        var response = await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dataContext.Database.BeginTransactionAsync(
                isolationLevel,
                cancellationToken);
            
            var response = await func(_repositoryRegistry);
            await _mediator.DispatchDomainEventsAsync(_dataContext, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return response;
        });

        return response;
    }

    public async Task ExecuteAsync(
        Func<IRepositoryRegistry, Task> action,
        CancellationToken cancellationToken = default)
    {
        await action(_repositoryRegistry);
        await _mediator.DispatchDomainEventsAsync(_dataContext, cancellationToken);
    }

    public async Task<TResponse> ExecuteAsync<TResponse>(
        Func<IRepositoryRegistry, Task<TResponse>> func,
        CancellationToken cancellationToken = default)
    {
        var response = await func(_repositoryRegistry);
        await _mediator.DispatchDomainEventsAsync(_dataContext, cancellationToken);
        return response;
    }
}