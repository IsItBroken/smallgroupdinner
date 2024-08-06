using Sgd.Domain.GroupAggregate;

namespace Sgd.Application.Groups.Queries.Common;

public class GroupResponse
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsOpen { get; init; }
    public DateTime CreatedAt { get; init; }
    public int MemberCount { get; init; }

    public static GroupResponse FromDomain(Group group)
    {
        return new GroupResponse
        {
            Id = group.Id.ToString(),
            Name = group.Name,
            Description = group.Description,
            ImageUrl = group.ImageUrl,
            IsOpen = group.IsOpen,
            CreatedAt = group.CreatedAt,
            MemberCount = group.MemberIds.Count
        };
    }
}
