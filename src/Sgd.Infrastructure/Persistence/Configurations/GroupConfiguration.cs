using Sgd.Domain.GroupAggregate;

namespace Sgd.Infrastructure.Persistence.Configurations;

public class GroupConfiguration : BsonClassMap<Group>
{
    public GroupConfiguration()
    {
        MapMember(a => a.Name).SetElementName(nameof(Group.Name).Camelize());

        MapMember(a => a.Description).SetElementName(nameof(Group.Description).Camelize());

        MapMember(a => a.IsOpen).SetElementName(nameof(Group.IsOpen).Camelize());

        MapMember(a => a.ImageUrl).SetElementName(nameof(Group.ImageUrl).Camelize());

        MapMember(a => a.CreatedAt).SetElementName(nameof(Group.CreatedAt).Camelize());

        MapMember(a => a.IsDeleted).SetElementName(nameof(Group.IsDeleted).Camelize());

        MapField("_memberIds").SetElementName("memberIds");
    }
}
