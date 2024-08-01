namespace Sgd.Domain.GroupAggregate;

public static class GroupErrors
{
    public static readonly Error UserAlreadyAssigned = Error.Conflict(
        "Group.UserAlreadyAssigned",
        "User is already assigned to the group"
    );
}
