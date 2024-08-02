using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Groups.Commands.UpdateGroup;

public record UpdateGroupCommand(
    ObjectId GroupId,
    string Name,
    string Description,
    string? ImageUrl,
    bool IsOpen
) : ICommand;
