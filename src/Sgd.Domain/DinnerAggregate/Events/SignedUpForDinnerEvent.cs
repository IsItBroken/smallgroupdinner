using Sgd.Domain.Common;

namespace Sgd.Domain.DinnerAggregate.Events;

public record SignedUpForDinnerEvent(Dinner Dinner, SignUp SignUp) : IDomainEvent;
