using Microsoft.Extensions.Logging;
using Sgd.Application.Common.Interfaces;
using Sgd.Domain.GroupAggregate.Events;

namespace Sgd.Application.Users.Events;

public class UserRemovedFromGroupEventHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<UserRemovedFromGroupEventHandler> logger
) : INotificationHandler<GroupProfileRemovedEvent>
{
    public async Task Handle(
        GroupProfileRemovedEvent notification,
        CancellationToken cancellationToken
    )
    {
        var user = await userRepository.GetUserById(notification.UserId);
        if (user is null)
        {
            logger.LogInformation("User {UserId} does not exist", notification.UserId);
            return;
        }

        user.RemoveGroup(notification.GroupId);
        userRepository.UpdateUser(user);
        await unitOfWork.CommitOperations();
    }
}
