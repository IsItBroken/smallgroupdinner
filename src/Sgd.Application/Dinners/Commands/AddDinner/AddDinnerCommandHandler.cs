using Sgd.Application.Common.Interfaces;
using Sgd.Application.Common.Messaging;
using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Dinners.Commands.AddDinner;

public class AddDinnerCommandHandler(IDinnerRepository dinnerRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<AddDinnerCommand, ObjectId>
{
    public async Task<ErrorOr<ObjectId>> Handle(
        AddDinnerCommand request,
        CancellationToken cancellationToken
    )
    {
        var dinner = new Dinner(request.Name, request.Date, request.Description, request.ImageUrl);
        dinnerRepository.AddDinner(dinner);

        await unitOfWork.CommitOperations();
        return dinner.Id;
    }
}
