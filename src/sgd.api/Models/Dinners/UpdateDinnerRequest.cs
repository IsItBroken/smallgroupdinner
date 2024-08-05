namespace Sgd.Api.Models.Dinners;

public record UpdateDinnerRequest(
    string Name,
    DateTime Date,
    string Description,
    int Capacity,
    string SignUpMethod,
    DateTime? RandomSelectionTime,
    string? ImageUrl
);
