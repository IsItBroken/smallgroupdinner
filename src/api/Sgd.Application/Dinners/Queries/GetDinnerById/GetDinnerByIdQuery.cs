using Sgd.Application.Common.Messaging;
using Sgd.Application.Dinners.Queries.Common;
using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Dinners.Queries.GetDinnerById;

public sealed record GetDinnerByIdQuery(ObjectId Id) : IQuery<DinnerResponse>;
