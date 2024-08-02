using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Application.Groups.Queries.Common;
using Sgd.Domain.GroupAggregate;

namespace Sgd.Application.Groups.Queries.GetGroupById;

public class GetGroupByIdQueryHandler(IGroupRepository groupRepository)
    : IQueryHandler<GetGroupByIdQuery, GroupResponse>
{
    public async Task<ErrorOr<GroupResponse>> Handle(
        GetGroupByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var group = await groupRepository.GetGroupById(request.GroupId, cancellationToken);
        if (group is null)
        {
            return GroupErrors.NotFound;
        }

        return GroupResponse.FromDomain(group);
    }
}
