namespace Sgd.Domain.Common.ValueObjects;

public class Address : ValueObject
{
    public string Line1 { get; private set; }
    public string? Line2 { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Zip { get; private set; }

    private Address(string line1, string? line2, string city, string state, string zip)
    {
        Line1 = line1;
        Line2 = line2;
        City = city;
        State = state;
        Zip = zip;
    }

    public static ErrorOr<Address> Create(
        string line1,
        string? line2,
        string city,
        string state,
        string zip
    )
    {
        return new Address(line1, line2, city, state, zip);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Line1;
        yield return Line2 ?? string.Empty;
        yield return City;
        yield return State;
        yield return Zip;
    }

    private Address() { }
}
