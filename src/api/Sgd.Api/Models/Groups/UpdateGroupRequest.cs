namespace Sgd.Api.Models.Groups;

public record UpdateGroupRequest(string Name, string Description, bool IsOpen, string? ImageUrl);
