using Sgd.Application.Common.Models;

namespace Sgd.Application.Common.Interfaces;

public interface ICurrentUserProvider
{
    public static readonly Error NotFoundError = Error.NotFound(
        "ApplicationUser.NotFound",
        "User not found."
    );

    CurrentUser? GetCurrentUser();

    IReadOnlyList<string> GetCurrentUserTokenPermissions();
}
