using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Users.Commands.AddUser;

public record AddUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string AliasSystem,
    string AliasValue,
    bool Blocking = false
) : ICommand<ObjectId>;
