using Sgd.Domain.Common;

namespace Sgd.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IDisposable Session { get; }

    void AddOperation(AggregateRoot<ObjectId> aggregateRoot, Action operation);

    void CleanOperations();

    void StartTransaction();

    Task CommitOperations();

    Task CommitTransaction();

    Task RollbackChanges();
}
