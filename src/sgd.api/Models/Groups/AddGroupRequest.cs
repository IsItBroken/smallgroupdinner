namespace Sgd.Api.Models.Groups;

public record AddGroupRequest(string Name, string Description, bool IsOpen, string? ImageUrl);
