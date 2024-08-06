using Sgd.Application.Common.Interfaces;
using Sgd.Domain.DinnerAggregate.Events;

namespace Sgd.Application.Dinners.Events;

public class UpdateDinnerAttendanceOnDinnerDateChangedEventHandler(
    IDinnerAttendanceRepository dinnerAttendanceRepository,
    IUnitOfWork unitOfWork
) : INotificationHandler<DinnerDateChangedEvent>
{
    public async Task Handle(
        DinnerDateChangedEvent notification,
        CancellationToken cancellationToken
    )
    {
        var dinnerAttendances = await dinnerAttendanceRepository.GetDinnerAttendancesForDinner(
            notification.Dinner.Id,
            cancellationToken
        );

        foreach (var dinnerAttendance in dinnerAttendances)
        {
            dinnerAttendance.UpdateDinnerDate(notification.Dinner.Date);
            dinnerAttendanceRepository.UpdateDinnerAttendance(dinnerAttendance);
        }

        await unitOfWork.CommitOperations();
    }
}
