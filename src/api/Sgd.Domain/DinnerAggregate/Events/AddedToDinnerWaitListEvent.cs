using Sgd.Domain.Common;

namespace Sgd.Domain.DinnerAggregate.Events;

public record AddedToDinnerWaitListEvent(Dinner Dinner, SignUp SignUp) : IDomainEvent;
