using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Dinners.Queries.SearchDinners;

public sealed record SearchDinnersQuery(string Name) : IQuery<IReadOnlyList<DinnerResponse>>;