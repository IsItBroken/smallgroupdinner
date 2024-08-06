namespace Sgd.Domain.UserAggregate;

public static class UserErrors
{
    public static readonly Error NotFound = Error.NotFound("User.NotFound", "User not found.");

    public static readonly Error EmailInUse = Error.NotFound(
        "User.EmailInUse",
        "The email is already in use."
    );

    public static readonly Error AliasAlreadyExists = Error.Conflict(
        "User.AliasExists",
        "The alias already exists for the user."
    );

    public static readonly Error GroupAlreadyAdded = Error.Conflict(
        "User.GroupAlreadyAdded",
        "The group is already added to the user."
    );

    public static readonly Error NotMemberOfGroup = Error.NotFound(
        "User.NotMemberOfGroup",
        "The user is not a member of the group."
    );
}
