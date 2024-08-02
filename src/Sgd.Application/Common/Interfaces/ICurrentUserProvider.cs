using Sgd.Application.Common.Models;
using Sgd.Domain.UserAggregate;

namespace Sgd.Application.Common.Interfaces;

public interface ICurrentUserProvider
{
    public static readonly Error NotFoundError = Error.NotFound(
        "ApplicationUser.NotFound",
        "User not found."
    );

    CurrentUser? GetCurrentUser();

    Task<ErrorOr<User>> GetUserDomain();

    IReadOnlyList<string> GetCurrentUserTokenPermissions();

    public string? GetTokenSub();
}
