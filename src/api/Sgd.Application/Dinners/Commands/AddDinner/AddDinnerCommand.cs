using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Dinners.Commands.AddDinner;

public sealed record AddDinnerCommand(
    string Name,
    DateTime Date,
    string Description,
    decimal? AveragePrice,
    ObjectId GroupId,
    int Capacity,
    string SignUpMethod,
    DateTime? RandomSelectionTime,
    string? ImageUrl
) : ICommand<ObjectId>;
