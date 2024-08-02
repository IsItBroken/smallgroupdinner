using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Domain.GroupAggregate;
using Sgd.Domain.UserAggregate;

namespace Sgd.Application.Groups.Commands.AddGroup;

public class AddGroupCommandHandler(
    IGroupRepository groupRepository,
    ICurrentUserProvider currentUserProvider,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<AddGroupCommand, ObjectId>
{
    public async Task<ErrorOr<ObjectId>> Handle(
        AddGroupCommand request,
        CancellationToken cancellationToken
    )
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        if (currentUser is null)
        {
            return UserErrors.NotFound;
        }

        var user = await userRepository.GetUserById(currentUser.Id);
        if (user is null)
        {
            return UserErrors.NotFound;
        }

        var group = Group.CreateGroup(
            request.Name,
            request.Description,
            request.IsOpen,
            request.ImageUrl,
            user
        );

        if (group.IsError)
        {
            return group.Errors;
        }

        groupRepository.AddGroup(group.Value);
        await unitOfWork.CommitOperations();

        return group.Value.Id;
    }
}
