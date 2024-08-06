using Sgd.Domain.Common;

namespace Sgd.Domain.UserAggregate;

public class UserAlias : ValueObject
{
    public string System { get; private set; } = string.Empty;
    public string Identifier { get; private set; } = string.Empty;

    public UserAlias(string system, string identifier)
    {
        System = system;
        Identifier = identifier;
    }

    private UserAlias() { }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return System;
        yield return Identifier;
    }
}
