using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Groups.Commands.AddGroup;

public record AddGroupCommand(string Name, string Description, bool IsOpen, string? ImageUrl)
    : ICommand<ObjectId>;
