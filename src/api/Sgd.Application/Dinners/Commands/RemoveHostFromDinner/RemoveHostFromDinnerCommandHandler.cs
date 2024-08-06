using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Dinners.Commands.RemoveHostFromDinner;

public class RemoveHostFromDinnerCommandHandler(
    IDinnerRepository dinnerRepository,
    ICurrentUserProvider currentUserProvider,
    IUnitOfWork unitOfWork
) : ICommandHandler<RemoveHostFromDinnerCommand>
{
    public async Task<ErrorOr<Success>> Handle(
        RemoveHostFromDinnerCommand request,
        CancellationToken cancellationToken
    )
    {
        var dinner = await dinnerRepository.GetDinnerById(request.Id, cancellationToken);
        if (dinner is null)
        {
            return DinnerErrors.NotFound;
        }

        var user = await currentUserProvider.GetUserDomain();
        if (user.IsError)
        {
            return user.Errors;
        }

        if (!dinner.CanUserUpdate(user.Value))
        {
            return DinnerErrors.CannotUpdateDinner;
        }

        var result = dinner.RemoveHost(request.HostId);
        if (result.IsError)
        {
            return result.Errors;
        }

        dinnerRepository.UpdateDinner(dinner);
        await unitOfWork.CommitOperations();
        return Result.Success;
    }
}
