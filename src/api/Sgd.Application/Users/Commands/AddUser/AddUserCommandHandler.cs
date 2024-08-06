using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Domain.UserAggregate;

namespace Sgd.Application.Users.Commands.AddUser;

public class AddUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<AddUserCommand, ObjectId>
{
    public async Task<ErrorOr<ObjectId>> Handle(
        AddUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var userWithEmailExists = await userRepository.ExistsByEmail(request.Email);
        if (userWithEmailExists)
        {
            return UserErrors.EmailInUse;
        }

        var user = new User(request.FirstName, request.LastName, request.Email);
        var alias = new UserAlias(request.AliasSystem, request.AliasValue);
        var addAliasResult = user.AddAlias(alias);
        if (addAliasResult.IsError)
        {
            return addAliasResult.Errors;
        }

        if (request.Blocking)
        {
            await userRepository.AddUserBlocking(user);
        }
        else
        {
            userRepository.AddUser(user);
            await unitOfWork.CommitOperations();
        }

        return user.Id;
    }
}
