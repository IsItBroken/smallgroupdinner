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
}
