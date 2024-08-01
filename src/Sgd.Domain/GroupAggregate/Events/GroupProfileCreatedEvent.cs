using Sgd.Domain.Common;

namespace Sgd.Domain.GroupAggregate.Events;

public record GroupProfileCreatedEvent(GroupProfile GroupProfile) : IDomainEvent;
