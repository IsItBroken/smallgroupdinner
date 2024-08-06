using Sgd.Domain.DinnerAggregate;

namespace Sgd.Infrastructure.Persistence.Configurations;

public class DinnerAttendanceConfiguration : BsonClassMap<DinnerAttendance>
{
    public DinnerAttendanceConfiguration()
    {
        MapMember(a => a.UserId).SetElementName(nameof(DinnerAttendance.UserId).Camelize());

        MapMember(a => a.DinnerId).SetElementName(nameof(DinnerAttendance.DinnerId).Camelize());

        MapMember(a => a.DinnerDate).SetElementName(nameof(DinnerAttendance.DinnerDate).Camelize());

        MapMember(a => a.IsUpcoming).SetElementName(nameof(DinnerAttendance.IsUpcoming).Camelize());
    }
}
