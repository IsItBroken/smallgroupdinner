using Sgd.Domain.Common;
using Sgd.Domain.GroupProfileAggregate;

namespace Sgd.Domain.GroupAggregate.Events;

public record GroupProfileCreatedEvent(GroupProfile GroupProfile) : IDomainEvent;
