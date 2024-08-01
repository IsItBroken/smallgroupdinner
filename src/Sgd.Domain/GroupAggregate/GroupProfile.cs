using Sgd.Domain.Common;

namespace Sgd.Domain.GroupAggregate;

public class GroupProfile : Entity<ObjectId>
{
    public ObjectId GroupId { get; private set; }

    public ObjectId UserId { get; private set; }

    public GroupRole Role { get; private set; }

    public bool NotifyOnNewDinner { get; private set; } = false;

    public GroupProfile(ObjectId groupId, ObjectId userId, GroupRole role, ObjectId? id = null)
        : base(id ?? ObjectId.GenerateNewId())
    {
        GroupId = groupId;
        UserId = userId;
        Role = role;
    }

    public ErrorOr<Success> SetNotifyOnNewDinner(bool notify)
    {
        NotifyOnNewDinner = notify;
        return Result.Success;
    }

    private GroupProfile() { }
}
