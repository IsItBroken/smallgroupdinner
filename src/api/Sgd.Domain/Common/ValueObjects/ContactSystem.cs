namespace Sgd.Domain.Common.ValueObjects;

public class ContactSystem(string name, string value)
    : SmartEnum<ContactSystem, string>(name, value)
{
    public static readonly ContactSystem Phone = new("Phone", "phone");
    public static readonly ContactSystem Email = new("Email", "email");
}
