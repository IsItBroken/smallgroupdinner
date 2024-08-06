namespace Sgd.Application.Groups.Commands.UpdateGroup;

public class UpdateGroupCommandValidator : AbstractValidator<UpdateGroupCommand>
{
    public UpdateGroupCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(150);
        RuleFor(x => x.Description).NotEmpty().MinimumLength(3).MaximumLength(1500);
        RuleFor(x => x.ImageUrl).MaximumLength(1000);
    }
}
