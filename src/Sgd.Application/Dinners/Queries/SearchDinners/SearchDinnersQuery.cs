using Sgd.Application.Common.Messaging;
using Sgd.Application.Dinners.Queries.Common;

namespace Sgd.Application.Dinners.Queries.SearchDinners;

public sealed record SearchDinnersQuery(string? Name) : IQuery<IReadOnlyList<DinnerResponse>>;
