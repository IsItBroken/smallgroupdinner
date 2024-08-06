using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Application.Groups.Services;
using Sgd.Domain.GroupAggregate;
using Sgd.Domain.GroupProfileAggregate;

namespace Sgd.Application.Groups.Commands.UpdateGroup;

public class UpdateGroupCommandHandler(
    IGroupRepository groupRepository,
    GroupMemberService groupMemberService,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateGroupCommand>
{
    public async Task<ErrorOr<Success>> Handle(
        UpdateGroupCommand request,
        CancellationToken cancellationToken
    )
    {
        var group = await groupRepository.GetGroupById(request.GroupId, cancellationToken);
        if (group is null)
        {
            return GroupErrors.NotFound;
        }

        if (!await groupMemberService.CurrentUserIsActiveMemberAndRole(group.Id, GroupRole.Admin))
        {
            return GroupErrors.GroupActionNotAllowed;
        }

        var updateResult = group.UpdateGroup(
            request.Name,
            request.Description,
            request.IsOpen,
            request.ImageUrl
        );

        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }

        groupRepository.UpdateGroup(group);
        await unitOfWork.CommitOperations();
        return Result.Success;
    }
}
