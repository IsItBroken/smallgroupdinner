namespace Sgd.Domain.Common;

public abstract class Entity<T>
{
    public T Id { get; private set; } = default!;

    public override bool Equals(object? other)
    {
        if (other == null || other.GetType() != GetType())
        {
            return false;
        }

        var otherEntity = (Entity<T>)other;
        return Equals(Id, otherEntity.Id);
    }

    public override int GetHashCode()
    {
        return Id?.GetHashCode() ?? 0;
    }

    protected Entity(T id) => Id = id;

    protected Entity() { }
}
