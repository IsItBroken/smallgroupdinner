using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Dinners.Services;

public static class DinnerUserHelper
{
    public static List<ObjectId> GetUserIdsInvolvedInDinners(IEnumerable<Dinner> dinners)
    {
        return dinners
            .SelectMany(dinner =>
                dinner
                    .Hosts.Concat(dinner.SignUps.Select(s => s.UserId))
                    .Concat(dinner.WaitList.Select(w => w.UserId))
            )
            .Distinct()
            .ToList();
    }

    public static List<ObjectId> GetUserIdsInvolvedInDinner(Dinner dinner)
    {
        return GetUserIdsInvolvedInDinners(new[] { dinner });
    }
}
