using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Dinners.Commands.CancelDinner;

public record CancelDinnerCommand(ObjectId Id) : ICommand;
