using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Dinners.Commands.UpdateDinner;

public record UpdateDinnerCommand(
    ObjectId Id,
    string Name,
    DateTime Date,
    string Description,
    decimal? AveragePrice,
    int Capacity,
    string SignUpMethod,
    DateTime? RandomSelectionTime,
    string? ImageUrl
) : ICommand;
