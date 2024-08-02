using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Dinners.Commands.SIgnUpForDinner;

public record SignUpForDinnerCommand(ObjectId DinnerId) : ICommand;
