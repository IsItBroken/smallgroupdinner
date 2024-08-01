namespace Sgd.Infrastructure.CIAM.Auth0;

public static class Auth0TokenUtility
{
    public static (string system, string value) GetSystemAndId(string value) =>
        value.Split('|') switch
        {
            { Length: 2 } => (value.Split('|')[0], value.Split('|')[1]),
            _ => throw new Exception("Invalid Auth0 PatientAccessId format")
        };
}
