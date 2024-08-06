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

    public static readonly Error NotMember = Error.Forbidden(
        "Group.NotMember",
        "User is not a member of the group"
    );

    public static readonly Error NotAdmin = Error.Forbidden(
        "Group.NotAdmin",
        "User is not an admin of the group"
    );

    public static readonly Error GroupActionNotAllowed = Error.Forbidden(
        "Group.GroupActionNotAllowed",
        "Group action is not allowed"
    );

    public static readonly Error NameInUse = Error.Conflict(
        "Group.NameInUse",
        "Group name is already in use"
    );

    public static readonly Error InvalidRole = Error.Validation(
        "Group.InvalidRole",
        "Invalid role"
    );
}
