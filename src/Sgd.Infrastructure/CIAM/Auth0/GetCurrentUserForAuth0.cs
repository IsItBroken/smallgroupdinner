using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Models;

namespace Sgd.Infrastructure.CIAM.Auth0;

public class GetCurrentUserForAuth0(
    IHttpContextAccessor httpContextAccessor,
    IUserRepository userRepository,
    IMemoryCache memoryCache,
    ILogger<GetCurrentUserForAuth0> logger
) : ICurrentUserProvider
{
    private const int CacheDurationMinutes = 15;

    private static string CacheKey(string system, string identifier) =>
        $"GetCurrentUserForAuth0-{system}-{identifier}";

    public CurrentUser? GetCurrentUser()
    {
        var userId = GetUserId().GetAwaiter().GetResult();
        if (userId.IsError)
        {
            return null;
        }

        var permissions = GetCurrentUserTokenPermissions();
        var roles = GetClaimValues(ClaimTypes.Role);
        return new CurrentUser(Id: userId.Value, Permissions: permissions, Roles: roles);
    }

    public IReadOnlyList<string> GetCurrentUserTokenPermissions()
    {
        return GetClaimValues("permissions");
    }

    public string? GetTokenSub()
    {
        var claimsPrincipal = httpContextAccessor.HttpContext?.User;
        var auth0UserId =
            claimsPrincipal?.FindFirst("sub")
            ?? claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier);
        return auth0UserId?.Value;
    }

    private async Task<ErrorOr<ObjectId>> GetUserId()
    {
        var auth0UserId = GetTokenSub();
        if (auth0UserId is null)
        {
            logger.LogDebug("Sub claim not found in user claims");
            return ICurrentUserProvider.NotFoundError;
        }

        var (system, identifier) = Auth0TokenUtility.GetSystemAndId(auth0UserId);
        if (string.IsNullOrEmpty(system) || string.IsNullOrEmpty(identifier))
        {
            logger.LogDebug(
                "Auth0 PatientAccessId is not in the expected format: {Auth0Id}",
                auth0UserId
            );
            return ICurrentUserProvider.NotFoundError;
        }

        if (
            memoryCache.TryGetValue(CacheKey(system, identifier), out ObjectId? cachedUserId)
            && cachedUserId is not null
        )
        {
            return cachedUserId.Value;
        }

        var user = await userRepository.GetUserByAlias(identifier, system);
        if (user is null)
        {
            logger.LogDebug(
                "User not found for system: {System} and identifier: {Identifier}",
                system,
                identifier
            );
            return ICurrentUserProvider.NotFoundError;
        }

        memoryCache.Set(
            CacheKey(system, identifier),
            user.Id,
            TimeSpan.FromMinutes(CacheDurationMinutes)
        );

        return user.Id;
    }

    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        return httpContextAccessor
            .HttpContext!.User.Claims.Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();
    }
}
