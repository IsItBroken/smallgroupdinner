namespace Sgd.Domain.DinnerAggregate;

public static class DinnerErrors
{
    public static Error NotFound = Error.NotFound("Dinner.NotFound", "Dinner not found.");

    public static Error RandomSelectionTimeMustBeBeforeDinnerDate = Error.Validation(
        "Dinner.RandomSelectionTimeMustBeBeforeDinnerDate",
        "Random selection time must be before dinner date."
    );

    public static Error HostAlreadyAdded = Error.Conflict(
        "Dinner.HostAlreadyAdded",
        "Host already added."
    );

    public static Error HostNotFound = Error.NotFound("Dinner.HostNotFound", "Host not found.");

    public static Error CannotRemoveLastHost = Error.Validation(
        "Dinner.CannotRemoveLastHost",
        "Cannot remove last host."
    );

    public static Error SignUpNotFound = Error.NotFound(
        "Dinner.SignUpNotFound",
        "Sign up not found."
    );

    public static Error AlreadyInWaitList = Error.Conflict(
        "Dinner.AlreadyInWaitList",
        "User is already in the wait list."
    );
}
