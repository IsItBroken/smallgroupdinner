using MongoDB.Bson;
using MongoDB.Driver;
using Sgd.Application.Common.Interfaces;
using Sgd.Domain.DinnerAggregate;

namespace Sgd.Infrastructure.Persistence.Repositories;

public class DinnerRepository(SgdDbContext dbContext, IUnitOfWork unitOfWork) : IDinnerRepository
{
    public void AddDinner(Dinner dinner)
    {
        unitOfWork.AddOperation(dinner, () => dbContext.Dinners.InsertOneAsync(dinner));
    }

    public void UpdateDinner(Dinner dinner)
    {
        unitOfWork.AddOperation(
            dinner,
            () => dbContext.Dinners.ReplaceOneAsync(x => x.Id == dinner.Id, dinner)
        );
    }

    public async Task<Dinner?> GetDinnerById(ObjectId id, CancellationToken cancellationToken)
    {
        return await dbContext.Dinners.Find(p => p.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Dinner>> SearchDinners(
        List<ObjectId> groupIds,
        string? name,
        CancellationToken cancellationToken
    )
    {
        var nameExpression = $"/.*{name}.*/i";
        var filter = Builders<Dinner>.Filter.And(
            Builders<Dinner>.Filter.In(p => p.GroupId, groupIds),
            Builders<Dinner>.Filter.Regex(p => p.Name, new BsonRegularExpression(nameExpression))
        );
        return await dbContext.Dinners.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<List<Dinner>> GetDinnersByIds(
        List<ObjectId> ids,
        CancellationToken cancellationToken
    )
    {
        return await dbContext.Dinners.Find(p => ids.Contains(p.Id)).ToListAsync(cancellationToken);
    }
}
