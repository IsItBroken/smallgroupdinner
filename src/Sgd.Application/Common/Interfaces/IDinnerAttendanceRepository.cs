using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Common.Interfaces;

public interface IDinnerAttendanceRepository
{
    void AddDinnerAttendance(DinnerAttendance dinnerAttendance);
    void UpdateDinnerAttendance(DinnerAttendance dinnerAttendance);
    Task<DinnerAttendance?> GetDinnerAttendanceById(
        ObjectId id,
        CancellationToken cancellationToken
    );
    Task<List<DinnerAttendance>> GetDinnerAttendancesForUser(
        ObjectId userId,
        CancellationToken cancellationToken
    );

    Task<bool> DoesDinnerAttendanceExist(
        ObjectId userId,
        ObjectId dinnerId,
        CancellationToken cancellationToken
    );

    Task<List<DinnerAttendance>> GetDinnerAttendancesForDinner(
        ObjectId dinnerId,
        CancellationToken cancellationToken
    );
}
