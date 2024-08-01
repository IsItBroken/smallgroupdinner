using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Application.Dinners.Queries.Common;

namespace Sgd.Application.Dinners.Queries.SearchDinners;

internal sealed class SearchDinnersQueryHandler(IDinnerRepository dinnerRepository)
    : IQueryHandler<SearchDinnersQuery, IReadOnlyList<DinnerResponse>>
{
    public async Task<ErrorOr<IReadOnlyList<DinnerResponse>>> Handle(
        SearchDinnersQuery request,
        CancellationToken cancellationToken
    )
    {
        var dinners = await dinnerRepository.SearchDinners(request.Name, cancellationToken);
        return dinners.Select(DinnerResponse.FromDomain).ToList();
    }
}
