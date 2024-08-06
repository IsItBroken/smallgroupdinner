using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Domain.GroupAggregate;
using Sgd.Domain.GroupProfileAggregate;
using Sgd.Domain.UserAggregate;

namespace Sgd.Application.Groups.Commands.JoinGroup;

public class JoinGroupCommandHandler(
    IGroupRepository groupRepository,
    ICurrentUserProvider currentUserProvider,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<JoinGroupCommand, JoinGroupResponse>
{
    public async Task<ErrorOr<JoinGroupResponse>> Handle(
        JoinGroupCommand request,
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

        var group = await groupRepository.GetGroupById(request.GroupId, cancellationToken);
        if (group is null)
        {
            return GroupErrors.NotFound;
        }

        var addMemberResult = group.AddMember(user, GroupRole.Member);
        if (addMemberResult.IsError)
        {
            return addMemberResult.Errors;
        }

        groupRepository.UpdateGroup(group);
        await unitOfWork.CommitOperations();

        return new JoinGroupResponse(addMemberResult.Value);
    }
}
