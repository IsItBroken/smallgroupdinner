namespace Sgd.Domain.GroupProfileAggregate;

public class GroupRole(string name, string value) : SmartEnum<GroupRole, string>(name, value)
{
    public const string AdminValue = "group.admin";
    public const string MemberValue = "group.member";

    public static readonly GroupRole Admin = new("Admin", AdminValue);
    public static readonly GroupRole Member = new("Member", MemberValue);

    public static bool AreValidRoles(List<string> roles)
    {
        return roles.All(role => Admin.Value == role || Member.Value == role);
    }

    public static bool IsPermitted(GroupRole requiredRole, List<GroupRole> assignedRoles)
    {
        if (requiredRole == Admin)
        {
            return assignedRoles.Contains(Admin);
        }

        if (requiredRole == Member)
        {
            return assignedRoles.Contains(Admin) || assignedRoles.Contains(Member);
        }

        return false;
    }
}
