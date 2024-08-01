using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Sgd.Infrastructure.CIAM.Auth0;

public class Auth0TokenService(
    HttpClient httpClient,
    IOptions<Auth0Configuration> auth0Options,
    IMemoryCache cache
) : ITokenService
{
    private readonly Auth0Configuration _auth0Options = auth0Options.Value;

    public async Task<ErrorOr<string>> GetAccessTokenAsync(string audience)
    {
        var cacheKey = $"access_token_{audience}";
        if (!cache.TryGetValue(cacheKey, out string? accessToken))
        {
            var tokenResult = await RequestNewTokenAsync(audience);
            if (tokenResult.IsError)
            {
                return tokenResult.Errors;
            }

            accessToken = tokenResult.Value.AccessToken;

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(
                TimeSpan.FromSeconds(tokenResult.Value.ExpiresIn - 60)
            ); // Set cache expiration 60 seconds before token expires

            cache.Set(cacheKey, accessToken, cacheEntryOptions);
        }

        if (string.IsNullOrEmpty(accessToken))
        {
            return Error.Unexpected("TokenService.EmptyToken", "Token is empty");
        }

        return accessToken;
    }

    private async Task<ErrorOr<TokenResponse>> RequestNewTokenAsync(string audience)
    {
        var discoveryDocument = await httpClient.GetDiscoveryDocumentAsync(_auth0Options.Authority);
        if (discoveryDocument.IsError)
        {
            throw new HttpRequestException(
                "Error retrieving discovery document: " + discoveryDocument.Error
            );
        }

        var tokenRequest = new ClientCredentialsTokenRequest
        {
            Address = discoveryDocument.TokenEndpoint,
            ClientId = _auth0Options.ClientId,
            ClientSecret = _auth0Options.ClientSecret,
            Scope = "UserManagement.Read UserManagement.Write"
        };

        tokenRequest.Parameters.Add("audience", audience);

        var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(tokenRequest);

        if (tokenResponse.IsError)
        {
            return Error.Unexpected(
                tokenResponse.Error ?? "TokenService.TokenRequestError",
                tokenResponse.ErrorDescription ?? "Error requesting token"
            );
        }

        return tokenResponse;
    }
}
