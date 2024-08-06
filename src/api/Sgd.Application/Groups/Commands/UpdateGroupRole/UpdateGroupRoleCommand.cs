using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Groups.Commands.UpdateGroupRole;

public record UpdateGroupRoleCommand(ObjectId GroupId, ObjectId UserId, string Role) : ICommand;
