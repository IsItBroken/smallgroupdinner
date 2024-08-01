using Sgd.Domain.Common;

namespace Sgd.Domain.DinnerAggregate;

public class Dinner : AggregateRoot<ObjectId>
{
    public string Name { get; private set; } = null!;

    public DateTime Date { get; private set; }

    public string Description { get; private set; } = null!;

    public string? ImageUrl { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public bool IsDeleted { get; private set; } = false;

    public Dinner(
        string name,
        DateTime date,
        string description,
        string? imageUrl,
        ObjectId? id = null
    )
        : base(id ?? ObjectId.GenerateNewId())
    {
        Name = name;
        Date = date;
        Description = description;
        ImageUrl = imageUrl;
    }

    private Dinner() { }
}
