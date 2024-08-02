namespace Sgd.Domain.GroupProfileAggregate;

public static class GroupProfileErrors
{
    public static Error NotFound = Error.NotFound(
        "GroupProfile.NotFound",
        "Group profile not found."
    );

    public static Error AlreadyActive = Error.Failure(
        "GroupProfile.AlreadyActive",
        "Group profile is already active."
    );
}
