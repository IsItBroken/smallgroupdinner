using Sgd.Domain.Common;

namespace Sgd.Domain.UserAggregate;

public class User : AggregateRoot<ObjectId>
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string Email { get; private set; } = null!;

    public Dictionary<string, UserAlias> Aliases { get; private set; } = new();

    public User(string firstName, string lastName, string email, ObjectId? id = null)
        : base(id ?? ObjectId.GenerateNewId())
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email.ToLower();
    }

    public ErrorOr<Success> AddAlias(UserAlias alias)
    {
        if (!Aliases.TryAdd(alias.System, alias))
        {
            return UserErrors.AliasAlreadyExists;
        }

        return Result.Success;
    }

    private User() { }
}
