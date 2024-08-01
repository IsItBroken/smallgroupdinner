namespace Sgd.Domain.Common.ValueObjects;

public class HumanName : ValueObject
{
    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }

    public string? MiddleName { get; private set; }

    public string? Suffix { get; private set; }

    public HumanName(string? firstName, string? lastName, string? middleName, string? suffix)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        Suffix = suffix;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName ?? string.Empty;
        yield return LastName ?? string.Empty;
        yield return MiddleName ?? string.Empty;
        yield return Suffix ?? string.Empty;
    }

    private HumanName() { }
}
