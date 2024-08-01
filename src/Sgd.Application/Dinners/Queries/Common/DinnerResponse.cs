using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Dinners.Queries.Common;

public sealed class DinnerResponse
{
    public string Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public string? ImageUrl { get; init; }

    public static DinnerResponse FromDomain(Dinner dinner)
    {
        return new DinnerResponse
        {
            Id = dinner.Id.ToString(),
            Name = dinner.Name,
            Description = dinner.Description,
            ImageUrl = dinner.ImageUrl
        };
    }
}
