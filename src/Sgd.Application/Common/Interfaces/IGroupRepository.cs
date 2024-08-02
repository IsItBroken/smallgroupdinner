using Sgd.Domain.GroupAggregate;

namespace Sgd.Application.Common.Interfaces;

public interface IGroupRepository
{
    void AddGroup(Group group);

    void UpdateGroup(Group group);

    Task<Group?> GetGroupById(ObjectId id, CancellationToken cancellationToken);

    Task<List<Group>> GetGroupsByIds(List<ObjectId> ids, CancellationToken cancellationToken);

    Task<List<Group>> SearchGroups(string? name, CancellationToken cancellationToken);
}
