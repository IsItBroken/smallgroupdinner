using Humanizer;
using MongoDB.Bson.Serialization;
using Sgd.Domain.DinnerAggregate;

namespace Sgd.Infrastructure.Persistence.Configurations;

public class DinnerConfiguration : BsonClassMap<Dinner>
{
    public DinnerConfiguration()
    {
        MapMember(a => a.Name).SetElementName(nameof(Dinner.Name).Camelize());
        
        MapMember(a => a.Description).SetElementName(nameof(Dinner.Description).Camelize());
        
        MapMember(a => a.ImageUrl).SetElementName(nameof(Dinner.ImageUrl).Camelize());
        
        MapMember(a => a.CreatedAt).SetElementName(nameof(Dinner.CreatedAt).Camelize());
        
        MapMember(a => a.IsDeleted).SetElementName(nameof(Dinner.IsDeleted).Camelize());
    }
}