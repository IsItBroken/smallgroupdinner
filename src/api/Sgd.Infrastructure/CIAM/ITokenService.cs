namespace Sgd.Infrastructure.CIAM;

public interface ITokenService
{
    Task<ErrorOr<string>> GetAccessTokenAsync(string audience);
}
