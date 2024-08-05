using Sgd.Domain.Common;

namespace Sgd.Domain.DinnerAggregate.Events;

public record DinnerDateChangedEvent(Dinner Dinner, DateTime OldDate) : IDomainEvent;
