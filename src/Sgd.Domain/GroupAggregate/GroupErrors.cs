namespace Sgd.Domain.GroupAggregate;

public static class GroupErrors
{
    public static readonly Error NotFound = Error.NotFound("Group.NotFound", "Group not found");

    public static readonly Error UserAlreadyAssigned = Error.Conflict(
        "Group.UserAlreadyAssigned",
        "User is already assigned to the group"
    );

    public static readonly Error UserNotAssigned = Error.NotFound(
        "Group.UserNotAssigned",
        "User is not assigned to the group"
    );

    public static readonly Error CannotRemoveLastMember = Error.Conflict(
        "Group.CannotRemoveLastMember",
        "Cannot remove the last member of the group"
    );

    public static readonly Error GroupAlreadyDeleted = Error.Conflict(
        "Group.GroupAlreadyDeleted",
        "Group is already deleted"
    );
}
