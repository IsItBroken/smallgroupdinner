using Sgd.Domain.UserAggregate;

namespace Sgd.Application.Dinners.Queries.Common;

public class UserResponse
{
    public string Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }

    public static UserResponse FromDomain(User user)
    {
        return new UserResponse
        {
            Id = user.Id.ToString(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }
}
