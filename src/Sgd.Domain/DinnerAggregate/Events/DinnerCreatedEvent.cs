using Sgd.Domain.Common;

namespace Sgd.Domain.DinnerAggregate.Events;

public record DinnerCreatedEvent(Dinner Dinner) : IDomainEvent;
