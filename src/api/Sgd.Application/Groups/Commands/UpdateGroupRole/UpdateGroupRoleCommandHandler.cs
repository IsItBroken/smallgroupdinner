using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Application.Groups.Services;
using Sgd.Domain.GroupAggregate;
using Sgd.Domain.GroupProfileAggregate;

namespace Sgd.Application.Groups.Commands.UpdateGroupRole;

public class UpdateGroupRoleCommandHandler(
    IGroupProfileRepository groupProfileRepository,
    GroupMemberService groupMemberService,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateGroupRoleCommand>
{
    public async Task<ErrorOr<Success>> Handle(
        UpdateGroupRoleCommand request,
        CancellationToken cancellationToken
    )
    {
        if (!GroupRole.IsValid(request.Role))
        {
            return GroupErrors.InvalidRole;
        }

        var groupProfile = await groupProfileRepository.GetGroupProfileByGroupAndUser(
            request.GroupId,
            request.UserId
        );

        if (groupProfile is null)
        {
            return GroupProfileErrors.NotFound;
        }

        if (
            !await groupMemberService.CurrentUserIsActiveMemberAndRole(
                groupProfile.GroupId,
                GroupRole.Admin
            )
        )
        {
            return GroupErrors.GroupActionNotAllowed;
        }

        var role = GroupRole.FromValue(request.Role);
        var updateResult = groupProfile.UpdateRole(role);
        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }

        groupProfileRepository.UpdateGroupProfile(groupProfile);
        await unitOfWork.CommitOperations();
        return Result.Success;
    }
}
