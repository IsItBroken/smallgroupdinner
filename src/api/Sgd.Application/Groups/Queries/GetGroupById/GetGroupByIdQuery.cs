using Sgd.Application.Common.Messaging;
using Sgd.Application.Groups.Queries.Common;

namespace Sgd.Application.Groups.Queries.GetGroupById;

public record GetGroupByIdQuery(ObjectId GroupId) : IQuery<GroupResponse>;
