using Sgd.Domain.Common;

namespace Sgd.Domain.DinnerAggregate;

public class Dinner : AggregateRoot<ObjectId>
{
    public string Name { get; private set; } = null!;

    public Dinner(string name, ObjectId? id = null)
        : base(id ?? ObjectId.GenerateNewId())
    {
        Name = name;
    }
    
    private Dinner() { }
}