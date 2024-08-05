using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Application.Dinners.Queries.Common;
using Sgd.Application.Dinners.Services;

namespace Sgd.Application.Dinners.Queries.SearchDinners;

internal sealed class SearchDinnersQueryHandler(
    IDinnerRepository dinnerRepository,
    IUserRepository userRepository
) : IQueryHandler<SearchDinnersQuery, IReadOnlyList<DinnerResponse>>
{
    public async Task<ErrorOr<IReadOnlyList<DinnerResponse>>> Handle(
        SearchDinnersQuery request,
        CancellationToken cancellationToken
    )
    {
        var dinners = await dinnerRepository.SearchDinners(request.Name, cancellationToken);

        var userIds = DinnerUserHelper.GetUserIdsInvolvedInDinners(dinners);
        var users = await userRepository.GetUsers(userIds);
        return dinners.Select(dinner => DinnerResponse.FromDomain(dinner, users)).ToList();
    }
}
