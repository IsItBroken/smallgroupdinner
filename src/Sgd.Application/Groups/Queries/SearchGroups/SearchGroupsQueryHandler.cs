using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Application.Groups.Queries.Common;

namespace Sgd.Application.Groups.Queries.SearchGroups;

public class SearchGroupsQueryHandler(IGroupRepository groupRepository)
    : IQueryHandler<SearchGroupsQuery, IReadOnlyList<GroupResponse>>
{
    public async Task<ErrorOr<IReadOnlyList<GroupResponse>>> Handle(
        SearchGroupsQuery request,
        CancellationToken cancellationToken
    )
    {
        var groups = await groupRepository.SearchGroups(request.Name, cancellationToken);
        return groups.Select(GroupResponse.FromDomain).ToList().AsReadOnly();
    }
}
