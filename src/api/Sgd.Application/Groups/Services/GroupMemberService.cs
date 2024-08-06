using Sgd.Application.Common.Interfaces;
using Sgd.Domain.GroupProfileAggregate;

namespace Sgd.Application.Groups.Services;

public class GroupMemberService(
    ICurrentUserProvider currentUserProvider,
    IGroupProfileRepository groupProfileRepository
)
{
    public async Task<bool> CurrentUserIsActiveMemberAndRole(ObjectId groupId, GroupRole role)
    {
        var currentUser = currentUserProvider.GetCurrentUser();
        if (currentUser == null)
        {
            return false;
        }

        var groupProfile = await groupProfileRepository.GetGroupProfileByGroupAndUser(
            groupId,
            currentUser.Id
        );

        if (groupProfile is null || !groupProfile.IsActive())
        {
            return false;
        }

        return GroupRole.IsPermitted(role, [groupProfile.Role]);
    }
}
