using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Application.Groups.Services;
using Sgd.Domain.GroupAggregate;
using Sgd.Domain.GroupProfileAggregate;

namespace Sgd.Application.Groups.Commands.AcceptGroupJoinRequest;

public class AcceptGroupJoinRequestCommandHandler(
    IGroupProfileRepository groupProfileRepository,
    GroupMemberService groupMemberService,
    IUnitOfWork unitOfWork
) : ICommandHandler<AcceptGroupJoinRequestCommand>
{
    public async Task<ErrorOr<Success>> Handle(
        AcceptGroupJoinRequestCommand request,
        CancellationToken cancellationToken
    )
    {
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

        var acceptResult = groupProfile.AcceptJoinRequest(request.Accepted);
        if (acceptResult.IsError)
        {
            return acceptResult.Errors;
        }

        groupProfileRepository.UpdateGroupProfile(groupProfile);
        await unitOfWork.CommitOperations();
        return Result.Success;
    }
}
