using Sgd.Domain.UserAggregate;

namespace Sgd.Application.Common.Interfaces;

public interface IUserRepository
{
    void AddUser(User user);
    Task AddUserBlocking(User user);
    Task<User?> GetUserById(ObjectId id);
    Task<List<User>> GetUsers(List<ObjectId> userIds);
    Task<User?> GetUserByAlias(string identifier, string system);
    void UpdateUser(User user);
    Task<bool> DoesUserWithAliasExists(string system, string identifier);
    Task<bool> ExistsByEmail(string email);
    Task<User?> GetUserByEmail(string email);
    Task<bool> DeleteUser(User user);
}
