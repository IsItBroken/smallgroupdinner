using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Domain.GroupAggregate;
using Sgd.Domain.UserAggregate;

namespace Sgd.Application.Groups.Commands.LeaveGroup;

public class LeaveGroupCommandHandler(
    IGroupRepository groupRepository,
    ICurrentUserProvider currentUserProvider,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<LeaveGroupCommand>
{
    public async Task<ErrorOr<Success>> Handle(
        LeaveGroupCommand request,
        CancellationToken cancellationToken
    )
    {
        var group = await groupRepository.GetGroupById(request.GroupId, cancellationToken);
        if (group is null)
        {
            return GroupErrors.NotFound;
        }

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

        var removeMemberResult = group.RemoveMember(user);
        if (removeMemberResult.IsError)
        {
            return removeMemberResult.Errors;
        }

        groupRepository.UpdateGroup(group);
        await unitOfWork.CommitOperations();

        return Result.Success;
    }
}
