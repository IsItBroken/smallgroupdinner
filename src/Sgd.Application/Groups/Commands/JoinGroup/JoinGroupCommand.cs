using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Groups.Commands.JoinGroup;

public record JoinGroupCommand(ObjectId GroupId) : ICommand<JoinGroupResponse>;
