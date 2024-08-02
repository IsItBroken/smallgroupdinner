namespace Sgd.Application.Groups.Queries.GetMembers;

public record GroupMemberResponse(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string Status,
    bool IsAdmin,
    bool IsPending
);
