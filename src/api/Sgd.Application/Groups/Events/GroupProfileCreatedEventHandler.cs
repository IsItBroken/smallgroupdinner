using Microsoft.Extensions.Logging;
using Sgd.Application.Common.Interfaces;
using Sgd.Domain.GroupAggregate.Events;

namespace Sgd.Application.Groups.Events;

public class GroupProfileCreatedEventHandler(
    IGroupProfileRepository groupProfileRepository,
    IUnitOfWork unitOfWork,
    ILogger<GroupProfileCreatedEventHandler> logger
) : INotificationHandler<GroupProfileCreatedEvent>
{
    public async Task Handle(
        GroupProfileCreatedEvent notification,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation(
            "GroupProfile {GroupProfileId} created",
            notification.GroupProfile.Id
        );

        groupProfileRepository.AddGroupProfile(notification.GroupProfile);
        await unitOfWork.CommitOperations();
    }
}
