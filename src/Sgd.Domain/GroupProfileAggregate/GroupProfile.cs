using Sgd.Domain.Common;
using Sgd.Domain.GroupProfileAggregate.Events;

namespace Sgd.Domain.GroupProfileAggregate;

public class GroupProfile : AggregateRoot<ObjectId>
{
    public ObjectId GroupId { get; private set; }

    public ObjectId UserId { get; private set; }

    public GroupRole Role { get; private set; }

    public GroupProfileStatus Status { get; private set; }

    public bool NotifyOnNewDinner { get; private set; } = false;

    public GroupProfile(
        ObjectId groupId,
        ObjectId userId,
        GroupRole role,
        GroupProfileStatus status,
        ObjectId? id = null
    )
        : base(id ?? ObjectId.GenerateNewId())
    {
        GroupId = groupId;
        UserId = userId;
        Role = role;
        Status = status;
    }

    public ErrorOr<Success> SetNotifyOnNewDinner(bool notify)
    {
        NotifyOnNewDinner = notify;
        return Result.Success;
    }

    public ErrorOr<Success> AcceptJoinRequest(bool accepted)
    {
        if (IsActive())
        {
            return GroupProfileErrors.AlreadyActive;
        }

        Status = accepted ? GroupProfileStatus.Active : GroupProfileStatus.Rejected;
        _domainEvents.Add(
            accepted
                ? new GroupInvitationAcceptedEvent(this)
                : new GroupInvitationRejectedEvent(this)
        );
        return Result.Success;
    }

    public bool IsActive()
    {
        return Status == GroupProfileStatus.Active;
    }

    private GroupProfile() { }
}
