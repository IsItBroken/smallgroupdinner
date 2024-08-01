namespace Sgd.Application.Dinners.Commands.AddDinner;

public sealed class AddDinnerCommandValidator : AbstractValidator<AddDinnerCommand>
{
    public AddDinnerCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(200);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().MinimumLength(3).MaximumLength(2500);
        RuleFor(x => x.ImageUrl).MaximumLength(500);
    }
}
