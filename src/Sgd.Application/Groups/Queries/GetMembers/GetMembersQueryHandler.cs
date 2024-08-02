using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Application.Groups.Services;
using Sgd.Domain.GroupAggregate;
using Sgd.Domain.GroupProfileAggregate;

namespace Sgd.Application.Groups.Queries.GetMembers;

public class GetMembersQueryHandler(
    IGroupRepository groupRepository,
    IGroupProfileRepository groupProfileRepository,
    GroupMemberService groupMemberService,
    IUserRepository userRepository
) : IQueryHandler<GetMembersQuery, List<GroupMemberResponse>>
{
    public async Task<ErrorOr<List<GroupMemberResponse>>> Handle(
        GetMembersQuery request,
        CancellationToken cancellationToken
    )
    {
        var group = await groupRepository.GetGroupById(request.GroupId, cancellationToken);
        if (group is null)
        {
            return GroupErrors.NotFound;
        }

        if (!await groupMemberService.CurrentUserIsActiveMemberAndRole(group.Id, GroupRole.Member))
        {
            return GroupErrors.GroupActionNotAllowed;
        }

        var groupProfiles = await groupProfileRepository.GetGroupProfilesByGroupId(group.Id);
        var memberResponses = new List<GroupMemberResponse>();

        foreach (var groupProfile in groupProfiles)
        {
            var user = await userRepository.GetUserById(groupProfile.UserId);
            if (user is null)
            {
                continue;
            }

            memberResponses.Add(
                new GroupMemberResponse(
                    groupProfile.UserId.ToString(),
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    groupProfile.Status.Value,
                    groupProfile.Role == GroupRole.Admin,
                    groupProfile.Status == GroupProfileStatus.Pending
                )
            );
        }

        return memberResponses;
    }
}
