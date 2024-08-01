namespace Sgd.Infrastructure.CIAM.Auth0;

public record Auth0Configuration(
    string Authority,
    string Audience,
    string ClientId,
    string ClientSecret,
    string UserManagementAudience
)
{
    public Auth0Configuration()
        : this("", "", "", "", "") { }
}
