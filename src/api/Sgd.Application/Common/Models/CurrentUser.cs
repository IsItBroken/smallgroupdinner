namespace Sgd.Application.Common.Models;

public record CurrentUser(
    ObjectId Id,
    IReadOnlyList<string> Permissions,
    IReadOnlyList<string> Roles
);
