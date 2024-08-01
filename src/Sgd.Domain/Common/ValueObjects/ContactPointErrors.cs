namespace Sgd.Domain.Common.ValueObjects;

public static class ContactPointErrors
{
    public static Error InvalidContactSystem = Error.Validation(
        "ContactPoint.InvalidContactSystem",
        "Invalid contact system."
    );
}
