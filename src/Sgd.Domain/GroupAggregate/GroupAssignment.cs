using Sgd.Domain.Common;
using Sgd.Domain.GroupProfileAggregate;

namespace Sgd.Domain.GroupAggregate;

public class GroupAssignment : ValueObject
{
    public ObjectId UserId { get; private set; }

    public GroupRole Role { get; private set; }

    public GroupAssignment(ObjectId userId, GroupRole role)
    {
        UserId = userId;
        Role = role;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return UserId;
        yield return Role;
    }

    private GroupAssignment() { }
}
