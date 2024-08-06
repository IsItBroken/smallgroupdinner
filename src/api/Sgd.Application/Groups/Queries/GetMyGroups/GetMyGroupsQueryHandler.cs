using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Application.Groups.Queries.Common;
using Sgd.Domain.UserAggregate;

namespace Sgd.Application.Groups.Queries.GetMyGroups;

public class GetMyGroupsQueryHandler(
    IUserRepository userRepository,
    IGroupRepository groupRepository,
    ICurrentUserProvider currentUserProvider
) : IQueryHandler<GetMyGroupsQuery, IReadOnlyList<GroupResponse>>
{
    public async Task<ErrorOr<IReadOnlyList<GroupResponse>>> Handle(
        GetMyGroupsQuery request,
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

        var groupIds = user.GroupIds.ToList();
        var groups = await groupRepository.GetGroupsByIds(groupIds, cancellationToken);

        return groups.Select(GroupResponse.FromDomain).ToList().AsReadOnly();
    }
}
