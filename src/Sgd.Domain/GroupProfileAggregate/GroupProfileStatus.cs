namespace Sgd.Domain.GroupProfileAggregate;

public class GroupProfileStatus(string name, string value)
    : SmartEnum<GroupProfileStatus, string>(name, value)
{
    public const string ActiveValue = "active";
    public const string PendingValue = "pending";
    public const string RejectedValue = "rejected";

    public static readonly GroupProfileStatus Active = new("Active", ActiveValue);
    public static readonly GroupProfileStatus Pending = new("Pending", PendingValue);
    public static readonly GroupProfileStatus Rejected = new("Rejected", RejectedValue);

    public static bool IsValidStatus(string status)
    {
        return Active.Value == status || Pending.Value == status || Rejected.Value == status;
    }
}
