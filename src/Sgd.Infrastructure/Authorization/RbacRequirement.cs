using Microsoft.AspNetCore.Authorization;

namespace Sgd.Infrastructure.Authorization;

public class RbacRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } =
        permission ?? throw new ArgumentNullException(nameof(permission));
}
