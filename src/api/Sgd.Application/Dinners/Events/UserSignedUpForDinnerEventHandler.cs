using Sgd.Application.Common.Interfaces;
using Sgd.Domain.DinnerAggregate;
using Sgd.Domain.DinnerAggregate.Events;

namespace Sgd.Application.Dinners.Events;

public class UserSignedUpForDinnerEventHandler(
    IDinnerAttendanceRepository dinnerAttendanceRepository,
    IUnitOfWork unitOfWork
) : INotificationHandler<SignedUpForDinnerEvent>
{
    public async Task Handle(
        SignedUpForDinnerEvent notification,
        CancellationToken cancellationToken
    )
    {
        if (
            await dinnerAttendanceRepository.DoesDinnerAttendanceExist(
                notification.SignUp.UserId,
                notification.Dinner.Id,
                cancellationToken
            )
        )
        {
            return;
        }

        var dinnerAttendance = new DinnerAttendance(
            notification.SignUp.UserId,
            notification.Dinner.Id,
            notification.Dinner.Date,
            true
        );

        dinnerAttendanceRepository.AddDinnerAttendance(dinnerAttendance);
        await unitOfWork.CommitOperations();
    }
}
