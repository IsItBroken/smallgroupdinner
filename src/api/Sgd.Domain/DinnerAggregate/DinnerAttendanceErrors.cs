namespace Sgd.Domain.DinnerAggregate;

public static class DinnerAttendanceErrors
{
    public static Error CannotUpdateDinnerDateToPast = Error.Validation(
        "DinnerAttendance.CannotUpdateDinnerDateToPast",
        "Cannot update dinner date to a past date."
    );
}
