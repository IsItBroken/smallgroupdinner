using Sgd.Domain.Common;

namespace Sgd.Domain.DinnerAggregate;

public class DinnerAttendance : AggregateRoot<ObjectId>
{
    public ObjectId UserId { get; private set; }
    public ObjectId DinnerId { get; private set; }
    public DateTime DinnerDate { get; private set; }
    public bool IsUpcoming { get; private set; }

    public DinnerAttendance(
        ObjectId userId,
        ObjectId dinnerId,
        DateTime dinnerDate,
        bool isUpcoming
    )
        : base(ObjectId.GenerateNewId())
    {
        UserId = userId;
        DinnerId = dinnerId;
        DinnerDate = dinnerDate;
        IsUpcoming = isUpcoming;
    }

    public void MarkAsPast()
    {
        IsUpcoming = false;
    }

    public void WasCanceled()
    {
        IsUpcoming = false;
    }

    public ErrorOr<Success> UpdateDinnerDate(DateTime newDinnerDate)
    {
        if (newDinnerDate < DateTime.UtcNow)
        {
            return DinnerAttendanceErrors.CannotUpdateDinnerDateToPast;
        }

        DinnerDate = newDinnerDate;
        return Result.Success;
    }

    private DinnerAttendance() { }
}
