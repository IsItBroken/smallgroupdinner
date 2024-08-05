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

    public static Error AlreadySignedUp = Error.Conflict(
        "Dinner.AlreadySignedUp",
        "User is already signed up."
    );

    public static Error CapacityCannotBeLessThanCurrentSignUps = Error.Validation(
        "Dinner.CapacityCannotBeLessThanCurrentSignUps",
        "Capacity cannot be less than current sign ups."
    );

    public static Error CapacityCannotBeNegative = Error.Validation(
        "Dinner.CapacityCannotBeNegative",
        "Capacity cannot be negative."
    );

    public static Error DinnerIsFull = Error.Validation("Dinner.DinnerIsFull", "Dinner is full.");

    public static Error CannotUpdateDinner = Error.Forbidden(
        "Dinner.CannotUpdateDinner",
        "Cannot update dinner."
    );

    public static Error DinnerAlreadyCanceled = Error.Conflict(
        "Dinner.DinnerAlreadyCanceled",
        "Dinner is already canceled."
    );

    public static Error DinnerAlreadyHappened = Error.Conflict(
        "Dinner.DinnerAlreadyHappened",
        "Dinner has already happened."
    );
}
