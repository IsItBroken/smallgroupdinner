using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Dinners.Commands.AddDinner;

public sealed record AddDinnerCommand(
    string Name,
    DateTime Date,
    string Description,
    string? ImageUrl
) : ICommand<ObjectId>;
