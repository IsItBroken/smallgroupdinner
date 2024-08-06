using Sgd.Application.Common.Messaging;
using Sgd.Application.Dinners.Queries.Common;

namespace Sgd.Application.Dinners.Queries.GetMyDinners;

public record GetMyDinnersQuery() : IQuery<IReadOnlyList<DinnerResponse>>;
