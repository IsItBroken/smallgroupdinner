using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Dinners.Commands.SIgnUpForDinner;

public class SignUpForDinnerCommandHandler(
    IDinnerRepository dinnerRepository,
    ICurrentUserProvider currentUserProvider,
    IUnitOfWork unitOfWork
) : ICommandHandler<SignUpForDinnerCommand>
{
    public async Task<ErrorOr<Success>> Handle(
        SignUpForDinnerCommand request,
        CancellationToken cancellationToken
    )
    {
        var dinner = await dinnerRepository.GetDinnerById(request.DinnerId, cancellationToken);
        if (dinner is null)
        {
            return DinnerErrors.NotFound;
        }

        var user = await currentUserProvider.GetUserDomain();
        if (user.IsError)
        {
            return user.Errors;
        }

        var signUp = new SignUp(user.Value.Id);
        var signUpResult = dinner.AddSignUp(signUp);
        if (signUpResult.IsError)
        {
            return signUpResult.Errors;
        }

        dinnerRepository.UpdateDinner(dinner);
        await unitOfWork.CommitOperations();

        return Result.Success;
    }
}
