namespace Sgd.Api.Models.Dinners;

public record UpdateDinnerRequest(
    string Name,
    DateTime Date,
    string Description,
    decimal? AveragePrice,
    int Capacity,
    string SignUpMethod,
    DateTime? RandomSelectionTime,
    string? ImageUrl
);
