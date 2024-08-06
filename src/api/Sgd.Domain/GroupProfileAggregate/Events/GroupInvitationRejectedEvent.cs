using Sgd.Domain.Common;

namespace Sgd.Domain.GroupProfileAggregate.Events;

public record GroupInvitationRejectedEvent(GroupProfile GroupProfile) : IDomainEvent;
