using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Dinners.Commands.RemoveHostFromDinner;

public record RemoveHostFromDinnerCommand(ObjectId Id, ObjectId HostId) : ICommand;
