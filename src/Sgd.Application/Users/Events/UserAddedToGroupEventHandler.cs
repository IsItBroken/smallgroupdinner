using Microsoft.Extensions.Logging;
using Sgd.Application.Common.Interfaces;
using Sgd.Domain.GroupAggregate.Events;

namespace Sgd.Application.Users.Events;

public class UserAddedToGroupEventHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<UserAddedToGroupEventHandler> logger
) : INotificationHandler<GroupProfileCreatedEvent>
{
    public async Task Handle(
        GroupProfileCreatedEvent notification,
        CancellationToken cancellationToken
    )
    {
        var user = await userRepository.GetUserById(notification.GroupProfile.UserId);
        if (user is null)
        {
            logger.LogInformation("User {UserId} does not exist", notification.GroupProfile.UserId);
            return;
        }

        user.AddGroup(notification.GroupProfile.GroupId);
        userRepository.UpdateUser(user);
        await unitOfWork.CommitOperations();
    }
}
