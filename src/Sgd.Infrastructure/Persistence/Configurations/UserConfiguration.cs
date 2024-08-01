using Sgd.Domain.UserAggregate;

namespace Sgd.Infrastructure.Persistence.Configurations;

public class UserConfiguration : BsonClassMap<User>
{
    public UserConfiguration()
    {
        MapMember(a => a.FirstName).SetElementName(nameof(User.FirstName).Camelize());

        MapMember(a => a.LastName).SetElementName(nameof(User.LastName).Camelize());

        MapMember(a => a.Email).SetElementName(nameof(User.Email).Camelize());

        MapMember(a => a.Aliases).SetElementName(nameof(User.Aliases).Camelize());
    }
}
