using Sgd.Application.Common.Messaging;
using Sgd.Application.Groups.Queries.Common;

namespace Sgd.Application.Groups.Queries.GetMyGroups;

public record GetMyGroupsQuery() : IQuery<IReadOnlyList<GroupResponse>>;
