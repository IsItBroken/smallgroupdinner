using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Dinners.Commands.CancelSignUpForDinner;

public record CancelSignUpForDinnerCommand(ObjectId Id) : ICommand;
