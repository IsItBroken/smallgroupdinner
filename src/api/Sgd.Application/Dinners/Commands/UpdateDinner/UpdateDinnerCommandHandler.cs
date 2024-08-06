using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Application.Groups.Services;
using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Dinners.Commands.UpdateDinner;

public class UpdateDinnerCommandHandler(
    IDinnerRepository dinnerRepository,
    ICurrentUserProvider currentUserProvider,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateDinnerCommand>
{
    public async Task<ErrorOr<Success>> Handle(
        UpdateDinnerCommand request,
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

        var errors = new List<Error>();
        if (dinner.Name != request.Name)
        {
            var updateNameRequest = dinner.UpdateName(request.Name);
            if (updateNameRequest.IsError)
            {
                errors.AddRange(updateNameRequest.Errors);
            }
        }

        if (dinner.Date != request.Date)
        {
            var updateDateRequest = dinner.UpdateDate(request.Date);
            if (updateDateRequest.IsError)
            {
                errors.AddRange(updateDateRequest.Errors);
            }
        }

        if (dinner.Description != request.Description)
        {
            var updateDescriptionRequest = dinner.UpdateDescription(request.Description);
            if (updateDescriptionRequest.IsError)
            {
                errors.AddRange(updateDescriptionRequest.Errors);
            }
        }

        if (dinner.Capacity != request.Capacity)
        {
            var updateCapacityRequest = dinner.UpdateCapacity(request.Capacity);
            if (updateCapacityRequest.IsError)
            {
                errors.AddRange(updateCapacityRequest.Errors);
            }
        }

        var signUpMethod = SignUpMethod.FromString(request.SignUpMethod);
        if (dinner.SignUpMethod.Name != signUpMethod.Name)
        {
            var updateSignUpMethodRequest = dinner.UpdateSignUpMethod(signUpMethod);
            if (updateSignUpMethodRequest.IsError)
            {
                errors.AddRange(updateSignUpMethodRequest.Errors);
            }
        }

        if (
            request.RandomSelectionTime is not null
            && dinner.RandomSelectionTime != request.RandomSelectionTime
        )
        {
            var updateRandomSelectionTimeRequest = dinner.UpdateRandomSelectionTime(
                request.RandomSelectionTime.Value
            );
            if (updateRandomSelectionTimeRequest.IsError)
            {
                errors.AddRange(updateRandomSelectionTimeRequest.Errors);
            }
        }

        if (dinner.ImageUrl != request.ImageUrl)
        {
            var updateImageUrlRequest = dinner.UpdateImageUrl(request.ImageUrl);
            if (updateImageUrlRequest.IsError)
            {
                errors.AddRange(updateImageUrlRequest.Errors);
            }
        }

        if (errors.Count != 0)
        {
            return errors;
        }

        dinnerRepository.UpdateDinner(dinner);
        await unitOfWork.CommitOperations();
        return Result.Success;
    }
}
