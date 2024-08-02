using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Dinners.Commands.AddDinner;

public class AddDinnerCommandHandler(
    IDinnerRepository dinnerRepository,
    ICurrentUserProvider currentUserProvider,
    IUnitOfWork unitOfWork
) : ICommandHandler<AddDinnerCommand, ObjectId>
{
    public async Task<ErrorOr<ObjectId>> Handle(
        AddDinnerCommand request,
        CancellationToken cancellationToken
    )
    {
        var currentUser = await currentUserProvider.GetUserDomain();
        if (currentUser.IsError)
        {
            return currentUser.Errors;
        }

        var signUpMethod = SignUpMethod.FromString(request.SignUpMethod);

        var dinner = Dinner.Create(
            request.Name,
            request.Date,
            request.Description,
            request.Capacity,
            signUpMethod,
            request.ImageUrl,
            request.RandomSelectionTime,
            currentUser.Value
        );

        dinnerRepository.AddDinner(dinner);

        await unitOfWork.CommitOperations();
        return dinner.Id;
    }
}
