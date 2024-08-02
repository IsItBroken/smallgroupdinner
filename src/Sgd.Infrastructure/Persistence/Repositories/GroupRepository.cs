using Sgd.Application.Common.Interfaces;
using Sgd.Domain.GroupAggregate;

namespace Sgd.Infrastructure.Persistence.Repositories;

public class GroupRepository(SgdDbContext dbContext, IUnitOfWork unitOfWork) : IGroupRepository
{
    public void AddGroup(Group group)
    {
        unitOfWork.AddOperation(group, () => dbContext.Groups.InsertOneAsync(group));
    }

    public void UpdateGroup(Group group)
    {
        unitOfWork.AddOperation(
            group,
            () => dbContext.Groups.ReplaceOneAsync(x => x.Id == group.Id, group)
        );
    }

    public async Task<Group?> GetGroupById(ObjectId id, CancellationToken cancellationToken)
    {
        return await dbContext.Groups.Find(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Group>> GetGroupsByIds(
        List<ObjectId> ids,
        CancellationToken cancellationToken
    )
    {
        var filter = Builders<Group>.Filter.In(x => x.Id, ids);
        return await dbContext.Groups.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<List<Group>> SearchGroups(string? name, CancellationToken cancellationToken)
    {
        var nameExpression = $"/.*{name}.*/i";
        var filter = Builders<Group>.Filter.Regex(o => o.Name, nameExpression);
        return await dbContext.Groups.Find(filter).ToListAsync(cancellationToken);
    }
}
