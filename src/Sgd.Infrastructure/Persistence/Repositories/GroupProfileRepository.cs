using Sgd.Application.Common.Interfaces;
using Sgd.Domain.GroupAggregate;
using Sgd.Domain.GroupProfileAggregate;

namespace Sgd.Infrastructure.Persistence.Repositories;

public class GroupProfileRepository(SgdDbContext dbContext, IUnitOfWork unitOfWork)
    : IGroupProfileRepository
{
    public void AddGroupProfile(GroupProfile groupProfile)
    {
        unitOfWork.AddOperation(
            groupProfile,
            () => dbContext.GroupProfiles.InsertOneAsync(groupProfile)
        );
    }

    public void UpdateGroupProfile(GroupProfile groupProfile)
    {
        unitOfWork.AddOperation(
            groupProfile,
            () =>
                dbContext.GroupProfiles.ReplaceOneAsync(x => x.Id == groupProfile.Id, groupProfile)
        );
    }

    public async Task<GroupProfile?> GetGroupProfileById(
        ObjectId id,
        CancellationToken cancellationToken
    )
    {
        return await dbContext
            .GroupProfiles.Find(p => p.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<GroupProfile>> GetGroupProfilesByGroupId(ObjectId groupId)
    {
        return await dbContext.GroupProfiles.Find(p => p.GroupId == groupId).ToListAsync();
    }

    public async Task<GroupProfile?> GetGroupProfileByGroupAndUser(
        ObjectId groupId,
        ObjectId userId
    )
    {
        return await dbContext
            .GroupProfiles.Find(p => p.GroupId == groupId && p.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public void DeleteGroupProfile(GroupProfile groupProfile)
    {
        unitOfWork.AddOperation(
            groupProfile,
            () => dbContext.GroupProfiles.DeleteOneAsync(x => x.Id == groupProfile.Id)
        );
    }
}
