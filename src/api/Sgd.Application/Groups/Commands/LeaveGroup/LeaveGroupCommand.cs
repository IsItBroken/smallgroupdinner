using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Groups.Commands.LeaveGroup;

public record LeaveGroupCommand(ObjectId GroupId) : ICommand;
