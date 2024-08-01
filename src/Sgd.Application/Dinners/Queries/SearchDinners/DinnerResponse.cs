using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Dinners.Queries.SearchDinners;

public sealed class DinnerResponse
{
    public string Id { get; init; }
    
    public string Name { get; init; }
    
    public static DinnerResponse FromDomain(Dinner dinner)
    {
        return new DinnerResponse
        {
            Id = dinner.Id.ToString(),
            Name = dinner.Name
        };
    }
}