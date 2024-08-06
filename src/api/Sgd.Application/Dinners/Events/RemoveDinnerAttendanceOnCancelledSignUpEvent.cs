using Sgd.Application.Common.Interfaces;
using Sgd.Domain.DinnerAggregate.Events;

namespace Sgd.Application.Dinners.Events;

public class RemoveDinnerAttendanceOnCancelledSignUpEvent(
    IDinnerAttendanceRepository dinnerAttendanceRepository,
    IUnitOfWork unitOfWork
) : INotificationHandler<CancelledSignUpEvent>
{
    public async Task Handle(CancelledSignUpEvent notification, CancellationToken cancellationToken)
    {
        var dinnerAttendance = await dinnerAttendanceRepository.GetDinnerAttendancesForDinner(
            notification.Dinner.Id,
            notification.SignUp.UserId,
            cancellationToken
        );

        if (dinnerAttendance is null)
        {
            return;
        }

        dinnerAttendanceRepository.RemoveDinnerAttendance(dinnerAttendance);
        await unitOfWork.CommitOperations();
    }
}
