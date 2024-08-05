using Sgd.Application.Common.Interfaces;
using Sgd.Domain.DinnerAggregate.Events;

namespace Sgd.Application.Dinners.Events;

public class UpdateDinnerAttendanceOnDinnerCanceledEvent(
    IDinnerAttendanceRepository dinnerAttendanceRepository,
    IUnitOfWork unitOfWork
) : INotificationHandler<DinnerCanceledEvent>
{
    public async Task Handle(DinnerCanceledEvent notification, CancellationToken cancellationToken)
    {
        var dinnerAttendances = await dinnerAttendanceRepository.GetDinnerAttendancesForDinner(
            notification.Dinner.Id,
            cancellationToken
        );

        foreach (var dinnerAttendance in dinnerAttendances)
        {
            dinnerAttendance.WasCanceled();
            dinnerAttendanceRepository.UpdateDinnerAttendance(dinnerAttendance);
        }

        await unitOfWork.CommitOperations();
    }
}
