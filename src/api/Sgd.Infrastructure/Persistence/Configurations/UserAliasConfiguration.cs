using Sgd.Domain.UserAggregate;

namespace Sgd.Infrastructure.Persistence.Configurations;

public class UserAliasConfiguration : BsonClassMap<UserAlias>
{
    public UserAliasConfiguration()
    {
        MapProperty(x => x.System).SetElementName(nameof(UserAlias.System).Camelize());
        MapProperty(x => x.Identifier).SetElementName(nameof(UserAlias.Identifier).Camelize());
    }
}
