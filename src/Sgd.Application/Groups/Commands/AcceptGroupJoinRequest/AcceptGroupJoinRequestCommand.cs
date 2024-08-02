using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Groups.Commands.AcceptGroupJoinRequest;

public record AcceptGroupJoinRequestCommand(ObjectId GroupId, ObjectId UserId, bool Accepted)
    : ICommand;
