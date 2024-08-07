using Sgd.Domain.DinnerAggregate;

namespace Sgd.Infrastructure.Persistence.Configurations;

public class DinnerConfiguration : BsonClassMap<Dinner>
{
    public DinnerConfiguration()
    {
        MapMember(a => a.Name).SetElementName(nameof(Dinner.Name).Camelize());

        MapMember(a => a.Date).SetElementName(nameof(Dinner.Date).Camelize());

        MapMember(a => a.Description).SetElementName(nameof(Dinner.Description).Camelize());
        
        MapMember(a => a.AveragePrice).SetElementName(nameof(Dinner.AveragePrice).Camelize());

        MapMember(a => a.GroupId).SetElementName(nameof(Dinner.GroupId).Camelize());

        MapMember(a => a.ImageUrl).SetElementName(nameof(Dinner.ImageUrl).Camelize());

        MapMember(a => a.Capacity).SetElementName(nameof(Dinner.Capacity).Camelize());

        MapMember(a => a.SignUpMethod).SetElementName(nameof(Dinner.SignUpMethod).Camelize());

        MapMember(a => a.RandomSelectionTime)
            .SetElementName(nameof(Dinner.RandomSelectionTime).Camelize());

        MapField("_signUps").SetElementName("signUps");

        MapField("_waitList").SetElementName("waitList");

        MapField("_hosts").SetElementName("hosts");

        MapMember(a => a.CreatedAt).SetElementName(nameof(Dinner.CreatedAt).Camelize());

        MapMember(a => a.IsCancelled).SetElementName(nameof(Dinner.IsCancelled).Camelize());
    }
}
