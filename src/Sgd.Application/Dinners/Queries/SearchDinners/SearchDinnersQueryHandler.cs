using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Dinners.Queries.SearchDinners;

internal sealed class SearchDinnersQueryHandler() : IQueryHandler<SearchDinnersQuery, IReadOnlyList<DinnerResponse>>
{
    public async Task<ErrorOr<IReadOnlyList<DinnerResponse>>> Handle(SearchDinnersQuery request, CancellationToken cancellationToken)
    {
        return new List<DinnerResponse>().AsReadOnly();
    }
}