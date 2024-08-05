using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Dinners.Commands.AddHostToDinner;

public record AddHostToDinnerCommand(ObjectId Id, ObjectId HostId) : ICommand;
