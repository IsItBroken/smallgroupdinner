using Sgd.Application.Common.Messaging;
using Sgd.Application.Groups.Queries.Common;

namespace Sgd.Application.Groups.Queries.SearchGroups;

public record SearchGroupsQuery(string? Name) : IQuery<IReadOnlyList<GroupResponse>>;
