using Sgd.Domain.Common;

namespace Sgd.Domain.DinnerAggregate.Events;

public record CancelledSignUpEvent(Dinner Dinner, SignUp SignUp) : IDomainEvent;
