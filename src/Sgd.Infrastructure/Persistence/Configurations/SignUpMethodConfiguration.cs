using Sgd.Domain.DinnerAggregate;

namespace Sgd.Infrastructure.Persistence.Configurations;

public class SignUpMethodConfiguration : BsonClassMap<SignUpMethod>
{
    public SignUpMethodConfiguration()
    {
        MapMember(a => a.Name).SetElementName(nameof(SignUpMethod.Name).Camelize());

        AddKnownType(typeof(FirstComeFirstServeMethod));
        AddKnownType(typeof(LotteryMethod));
        SetDiscriminatorIsRequired(true);
        SetDiscriminator(nameof(SignUpMethod.Name).Camelize());
    }
}

public class FirstComeFirstServeMethodConfiguration : BsonClassMap<FirstComeFirstServeMethod>
{
    public FirstComeFirstServeMethodConfiguration()
    {
        SetDiscriminator(nameof(FirstComeFirstServeMethod).Camelize());
    }
}

public class LotteryMethodConfiguration : BsonClassMap<LotteryMethod>
{
    public LotteryMethodConfiguration()
    {
        SetDiscriminator(nameof(LotteryMethod).Camelize());
    }
}
