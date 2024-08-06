using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Application.Dinners.Queries.Common;
using Sgd.Application.Dinners.Services;
using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Dinners.Queries.GetDinnerById;

public sealed class GetDinnerByIdQueryHandler(
    IDinnerRepository dinnerRepository,
    IUserRepository userRepository,
    ICurrentUserProvider currentUserProvider
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

        var currentUser = await currentUserProvider.GetUserDomain();
        if (currentUser.IsError)
        {
            return currentUser.Errors;
        }

        if (!currentUser.Value.IsMemberOfGroup(dinner.GroupId))
        {
            return DinnerErrors.Forbidden;
        }

        var userIds = DinnerUserHelper.GetUserIdsInvolvedInDinner(dinner);
        var users = await userRepository.GetUsers(userIds);

        return DinnerResponse.FromDomain(dinner, users);
    }
}
