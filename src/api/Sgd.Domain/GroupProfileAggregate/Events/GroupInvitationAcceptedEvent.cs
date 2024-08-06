using Sgd.Domain.Common;

namespace Sgd.Domain.GroupProfileAggregate.Events;

public record GroupInvitationAcceptedEvent(GroupProfile GroupProfile) : IDomainEvent;
