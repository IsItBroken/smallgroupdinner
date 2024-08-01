using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Sgd.Application.Common.Interfaces;
using Sgd.Domain.Common;
using Sgd.Domain.DinnerAggregate;
using Sgd.Domain.UserAggregate;
using Sgd.Infrastructure.Middleware;

namespace Sgd.Infrastructure.Persistence;

public class SgdDbContext : IUnitOfWork
{
    private static bool? _isTransactionSupported;

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPublisher _publisher;
    private readonly IMongoDatabase _database;

    private readonly IClientSessionHandle _session;
    public IDisposable Session => _session;

    private readonly List<Action> _operations = [];
    private readonly List<AggregateRoot<ObjectId>> _updatedAggregateRootsInSession = [];

    public IMongoCollection<Dinner> Dinners => _database.GetCollection<Dinner>("dinners");
    public IMongoCollection<User> Users => _database.GetCollection<User>("users");

    public SgdDbContext(
        IMongoClient client,
        IOptions<SgdDbOptions> options,
        IHttpContextAccessor httpContextAccessor,
        IPublisher publisher
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _publisher = publisher;

        _database = client.GetDatabase(options.Value.DatabaseName);
        _session = client.StartSession();
    }

    public void AddOperation(AggregateRoot<ObjectId> aggregateRoot, Action operation)
    {
        _updatedAggregateRootsInSession.Add(aggregateRoot);
        _operations.Add(operation);
    }

    public void CleanOperations()
    {
        _operations.Clear();
    }

    public async Task CommitOperations()
    {
        var domainEvents = _updatedAggregateRootsInSession
            .Select(entry => entry.PopDomainEvents())
            .SelectMany(x => x)
            .ToList();

        if (IsUserWaitingOnline())
        {
            AddDomainEventsToOfflineProcessingQueue(domainEvents);
            InvokeOperations();
            return;
        }

        await PublishDomainEvents(domainEvents);
        InvokeOperations();
    }

    public void StartTransaction()
    {
        if (_isTransactionSupported is not null && !_isTransactionSupported.Value)
            return;

        try
        {
            _session.StartTransaction();
        }
        catch (NotSupportedException)
        {
            _isTransactionSupported = false;
        }
    }

    public async Task CommitTransaction()
    {
        if (_isTransactionSupported is false)
            return;

        await _session.CommitTransactionAsync();
    }

    private void InvokeOperations()
    {
        _operations.ForEach(o =>
        {
            o.Invoke();
        });

        CleanOperations();
    }

    public async Task RollbackChanges()
    {
        if (_isTransactionSupported is true)
        {
            await _session.AbortTransactionAsync();
        }
    }

    private bool IsUserWaitingOnline() => _httpContextAccessor.HttpContext is not null;

    private async Task PublishDomainEvents(List<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
    }

    private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            throw new InvalidOperationException("HttpContext is null");
        }

        Queue<IDomainEvent> domainEventsQueue =
            _httpContextAccessor.HttpContext.Items.TryGetValue(
                EventualConsistencyMiddleware.DomainEventsKey,
                out var value
            ) && value is Queue<IDomainEvent> existingDomainEvents
                ? existingDomainEvents
                : new();

        domainEvents.ForEach(domainEventsQueue.Enqueue);
        _httpContextAccessor.HttpContext.Items[EventualConsistencyMiddleware.DomainEventsKey] =
            domainEventsQueue;
    }
}
