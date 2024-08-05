using Sgd.Domain.Common;

namespace Sgd.Domain.DinnerAggregate.Events;

public record DinnerCanceledEvent(Dinner Dinner) : IDomainEvent;
