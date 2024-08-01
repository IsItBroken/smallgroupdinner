using Sgd.Application.Common.Interfaces;
using Sgd.Domain.UserAggregate;

namespace Sgd.Infrastructure.Persistence.Repositories;

public class UserRepository(SgdDbContext dbContext, IUnitOfWork unitOfWork) : IUserRepository
{
    public void AddUser(User user)
    {
        unitOfWork.AddOperation(user, () => dbContext.Users.InsertOneAsync(user));
    }

    public async Task AddUserBlocking(User user)
    {
        await dbContext.Users.InsertOneAsync(user);
    }

    public async Task<User?> GetUserById(ObjectId id)
    {
        return await dbContext.Users.Find(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetUsers(List<ObjectId> userIds)
    {
        return await dbContext.Users.Find(u => userIds.Contains(u.Id)).ToListAsync();
    }

    public async Task<User?> GetUserByAlias(string identifier, string system)
    {
        var filter = Builders<User>.Filter.And(
            Builders<User>.Filter.Eq($"aliases.{system}.system", system),
            Builders<User>.Filter.Eq($"aliases.{system}.identifier", identifier)
        );

        var result = await dbContext.Users.Find(filter).FirstOrDefaultAsync();
        return result;
    }

    public void UpdateUser(User user)
    {
        unitOfWork.AddOperation(
            user,
            () => dbContext.Users.ReplaceOneAsync(u => u.Id == user.Id, user)
        );
    }

    public async Task<bool> DoesUserWithAliasExists(string system, string identifier)
    {
        var filter = Builders<User>.Filter.And(
            Builders<User>.Filter.Eq($"aliases.{system}.system", system),
            Builders<User>.Filter.Eq($"aliases.{system}.identifier", identifier)
        );

        var result = await dbContext.Users.Find(filter).AnyAsync();

        return result;
    }

    public async Task<bool> ExistsByEmail(string email)
    {
        return await dbContext.Users.Find(u => u.Email.ToLower() == email.ToLower()).AnyAsync();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await dbContext
            .Users.Find(u => u.Email.ToLower() == email.ToLower())
            .FirstOrDefaultAsync();
    }

    public async Task<bool> DeleteUser(User user)
    {
        var result = await dbContext.Users.DeleteOneAsync(u => u.Id == user.Id);
        return result.DeletedCount > 0;
    }
}
