namespace Sgd.Api.Models.Dinners;

public record AddDinnerRequest(string Name, DateTime Date, string Description, string? ImageUrl);
