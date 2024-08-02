using Sgd.Domain.GroupAggregate;
using Sgd.Domain.GroupProfileAggregate;

namespace Sgd.Application.Common.Interfaces;

public interface IGroupProfileRepository
{
    void AddGroupProfile(GroupProfile groupProfile);

    void UpdateGroupProfile(GroupProfile groupProfile);

    Task<GroupProfile?> GetGroupProfileById(ObjectId id, CancellationToken cancellationToken);

    Task<GroupProfile?> GetGroupProfileByGroupAndUser(ObjectId groupId, ObjectId userId);

    void DeleteGroupProfile(GroupProfile groupProfile);
}
