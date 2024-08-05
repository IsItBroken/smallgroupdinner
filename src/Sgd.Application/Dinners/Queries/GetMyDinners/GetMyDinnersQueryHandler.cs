using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Application.Dinners.Queries.Common;
using Sgd.Application.Dinners.Services;

namespace Sgd.Application.Dinners.Queries.GetMyDinners;

public class GetMyDinnersQueryHandler(
    IDinnerAttendanceRepository dinnerAttendanceRepository,
    IDinnerRepository dinnerRepository,
    ICurrentUserProvider currentUserProvider,
    IUserRepository userRepository
) : IQueryHandler<GetMyDinnersQuery, IReadOnlyList<DinnerResponse>>
{
    public async Task<ErrorOr<IReadOnlyList<DinnerResponse>>> Handle(
        GetMyDinnersQuery request,
        CancellationToken cancellationToken
    )
    {
        var user = await currentUserProvider.GetUserDomain();
        if (user.IsError)
        {
            return user.Errors;
        }

        var dinnerAttendances = await dinnerAttendanceRepository.GetDinnerAttendancesForUser(
            user.Value.Id,
            cancellationToken
        );

        var dinners = await dinnerRepository.GetDinnersByIds(
            dinnerAttendances.Select(x => x.DinnerId).ToList(),
            cancellationToken
        );

        var userIds = DinnerUserHelper.GetUserIdsInvolvedInDinners(dinners);
        var users = await userRepository.GetUsers(userIds);
        return dinners
            .Select(dinner => DinnerResponse.FromDomain(dinner, users))
            .OrderByDescending(d => d.Date)
            .ToList();
    }
}
