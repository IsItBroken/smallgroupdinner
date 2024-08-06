using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Groups.Queries.GetMembers;

public record GetMembersQuery(ObjectId GroupId) : IQuery<List<GroupMemberResponse>>;
