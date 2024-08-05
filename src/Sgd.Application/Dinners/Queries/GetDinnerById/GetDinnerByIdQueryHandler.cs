using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Application.Dinners.Queries.Common;
using Sgd.Application.Dinners.Services;
using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Dinners.Queries.GetDinnerById;

public sealed class GetDinnerByIdQueryHandler(
    IDinnerRepository dinnerRepository,
    IUserRepository userRepository
) : IQueryHandler<GetDinnerByIdQuery, DinnerResponse>
{
    public async Task<ErrorOr<DinnerResponse>> Handle(
        GetDinnerByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var dinner = await dinnerRepository.GetDinnerById(request.Id, cancellationToken);
        if (dinner is null)
        {
            return DinnerErrors.NotFound;
        }

        var userIds = DinnerUserHelper.GetUserIdsInvolvedInDinner(dinner);
        var users = await userRepository.GetUsers(userIds);

        return DinnerResponse.FromDomain(dinner, users);
    }
}
