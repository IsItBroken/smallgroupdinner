namespace Sgd.Api.Models.Dinners;

public record AddDinnerRequest(
    string Name,
    DateTime Date,
    string Description,
    string GroupId,
    int Capacity,
    string SignUpMethod,
    DateTime? RandomSelectionTime,
    string? ImageUrl
);
