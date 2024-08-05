using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Dinners.Commands.CancelSignUpForDinner;

public class CancelSignUpForDinnerCommandHandler(
    IDinnerRepository dinnerRepository,
    ICurrentUserProvider currentUserProvider,
    IUnitOfWork unitOfWork
) : ICommandHandler<CancelSignUpForDinnerCommand>
{
    public async Task<ErrorOr<Success>> Handle(
        CancelSignUpForDinnerCommand request,
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

        var result = dinner.RemoveSignUp(user.Value.Id);
        if (result.IsError)
        {
            return result.Errors;
        }

        await unitOfWork.CommitOperations();
        return Result.Success;
    }
}
