namespace Sgd.Application.Groups.Commands.AddGroup;

public class AddGroupCommandValidator : AbstractValidator<AddGroupCommand>
{
    public AddGroupCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(150);
        RuleFor(x => x.Description).NotEmpty().MinimumLength(3).MaximumLength(1500);
        RuleFor(x => x.ImageUrl).MaximumLength(1000);
    }
}
