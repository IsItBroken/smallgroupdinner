using Sgd.Domain.Common;

namespace Sgd.Domain.DinnerAggregate;

public class SignUp : ValueObject
{
    public ObjectId UserId { get; private set; }

    public DateTime SignUpDate { get; private set; } = DateTime.UtcNow;

    public SignUp(ObjectId userId)
    {
        UserId = userId;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return UserId;
    }

    private SignUp() { }
}
