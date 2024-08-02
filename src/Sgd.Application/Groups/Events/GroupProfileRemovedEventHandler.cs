using Microsoft.Extensions.Logging;
using Sgd.Application.Common.Interfaces;
using Sgd.Domain.GroupAggregate.Events;

namespace Sgd.Application.Groups.Events;

public class GroupProfileRemovedEventHandler(
    IGroupProfileRepository groupProfileRepository,
    IUnitOfWork unitOfWork,
    ILogger<GroupProfileRemovedEventHandler> logger
) : INotificationHandler<GroupProfileRemovedEvent>
{
    public async Task Handle(
        GroupProfileRemovedEvent notification,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation(
            "Group {GroupId} removed from User {UserId}",
            notification.GroupId,
            notification.UserId
        );

        var groupProfile = await groupProfileRepository.GetGroupProfileByGroupAndUser(
            notification.GroupId,
            notification.UserId
        );

        if (groupProfile is null)
        {
            logger.LogInformation(
                "Group {GroupId} not found for User {UserId}",
                notification.GroupId,
                notification.UserId
            );

            return;
        }

        groupProfileRepository.DeleteGroupProfile(groupProfile);
        await unitOfWork.CommitOperations();
    }
}
