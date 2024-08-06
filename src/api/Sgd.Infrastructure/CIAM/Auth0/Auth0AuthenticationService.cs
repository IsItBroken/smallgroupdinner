using Auth0.ManagementApi;

namespace Sgd.Infrastructure.CIAM.Auth0;

public class Auth0AuthenticationService(IManagementApiClient managementApiClient)
{
    public async Task<UserResponse?> GetUser(string identifier)
    {
        var user = await managementApiClient.Users.GetAsync(identifier);
        return user is null ? null : new UserResponse(user.FirstName, user.LastName, user.Email);
    }
}
