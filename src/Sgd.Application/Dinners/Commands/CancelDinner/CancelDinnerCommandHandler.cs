using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Dinners.Commands.CancelDinner;

public class CancelDinnerCommandHandler(
    IDinnerRepository dinnerRepository,
    ICurrentUserProvider currentUserProvider,
    IUnitOfWork unitOfWork
) : ICommandHandler<CancelDinnerCommand>
{
    public async Task<ErrorOr<Success>> Handle(
        CancelDinnerCommand request,
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

        var result = dinner.CancelDinner();
        if (result.IsError)
        {
            return result.Errors;
        }

        await unitOfWork.CommitOperations();
        return Result.Success;
    }
}
