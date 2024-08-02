using Sgd.Domain.Common;

namespace Sgd.Domain.GroupAggregate.Events;

public record GroupProfileRemovedEvent(ObjectId GroupId, ObjectId UserId) : IDomainEvent;
