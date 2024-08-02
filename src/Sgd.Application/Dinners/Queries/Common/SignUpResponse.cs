using Sgd.Domain.DinnerAggregate;
using Sgd.Domain.UserAggregate;

namespace Sgd.Application.Dinners.Queries.Common;

public sealed class SignUpResponse
{
    public string UserId { get; init; } = string.Empty;
    public DateTime SignUpDate { get; init; }
    public UserResponse? User { get; init; }

    public static SignUpResponse FromDomain(SignUp signUp, User user)
    {
        return new SignUpResponse
        {
            UserId = signUp.UserId.ToString(),
            SignUpDate = signUp.SignUpDate,
            User = UserResponse.FromDomain(user)
        };
    }
}
