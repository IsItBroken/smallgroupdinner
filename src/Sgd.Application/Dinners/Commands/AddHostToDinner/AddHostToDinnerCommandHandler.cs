using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Dinners.Commands.AddHostToDinner;

public class AddHostToDinnerCommandHandler(
    IDinnerRepository dinnerRepository,
    ICurrentUserProvider currentUserProvider,
    IUnitOfWork unitOfWork
) : ICommandHandler<AddHostToDinnerCommand>
{
    public async Task<ErrorOr<Success>> Handle(
        AddHostToDinnerCommand request,
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

        var result = dinner.AddHost(request.HostId);
        if (result.IsError)
        {
            return result.Errors;
        }

        await unitOfWork.CommitOperations();
        return Result.Success;
    }
}
