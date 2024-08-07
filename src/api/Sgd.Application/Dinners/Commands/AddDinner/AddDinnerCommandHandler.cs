using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Application.Groups.Services;
using Sgd.Domain.DinnerAggregate;
using Sgd.Domain.GroupAggregate;
using Sgd.Domain.GroupProfileAggregate;

namespace Sgd.Application.Dinners.Commands.AddDinner;

public class AddDinnerCommandHandler(
    IDinnerRepository dinnerRepository,
    ICurrentUserProvider currentUserProvider,
    GroupMemberService groupMemberService,
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

        if (
            !await groupMemberService.CurrentUserIsActiveMemberAndRole(
                request.GroupId,
                GroupRole.Member
            )
        )
        {
            return GroupErrors.NotMember;
        }

        var signUpMethod = SignUpMethod.FromString(request.SignUpMethod);

        var dinner = Dinner.Create(
            request.Name,
            request.Date,
            request.Description,
            request.AveragePrice,
            request.GroupId,
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
