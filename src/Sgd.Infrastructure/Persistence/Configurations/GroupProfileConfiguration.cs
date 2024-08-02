using Sgd.Domain.GroupProfileAggregate;

namespace Sgd.Infrastructure.Persistence.Configurations;

public class GroupProfileConfiguration : BsonClassMap<GroupProfile>
{
    public GroupProfileConfiguration()
    {
        MapMember(a => a.GroupId).SetElementName(nameof(GroupProfile.GroupId).Camelize());
        MapMember(a => a.UserId).SetElementName(nameof(GroupProfile.UserId).Camelize());
        MapMember(a => a.Role).SetElementName(nameof(GroupProfile.Role).Camelize());
        MapMember(a => a.NotifyOnNewDinner)
            .SetElementName(nameof(GroupProfile.NotifyOnNewDinner).Camelize());
    }
}
