namespace Sgd.Domain.Common.ValueObjects;

public class ContactPoint : ValueObject
{
    public ContactSystem System { get; private set; }

    public string Value { get; private set; }

    public int? Rank { get; private set; }

    public ContactPoint(ContactSystem system, string value, int? rank)
    {
        System = system;
        Value = value;
        Rank = rank;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return System;
        yield return Value;
    }

    private ContactPoint() { }
}
