namespace Sgd.Application.Dinners.Commands.UpdateDinner;

public class UpdateDinnerCommandValidator : AbstractValidator<UpdateDinnerCommand>
{
    private readonly string[] _validSignUpMethods = { "FirstComeFirstServe", "Lottery" };

    public UpdateDinnerCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(200);
        RuleFor(x => x.Date).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().MinimumLength(3).MaximumLength(2500);
        RuleFor(x => x.ImageUrl).MaximumLength(500);
        RuleFor(x => x.Capacity).GreaterThan(0);
        RuleFor(x => x.SignUpMethod)
            .Must(BeAValidSignUpMethod)
            .WithMessage(x =>
                $"Invalid SignUpMethod. Valid types are: {string.Join(", ", _validSignUpMethods)}."
            );
    }

    private bool BeAValidSignUpMethod(string signUpMethod)
    {
        return _validSignUpMethods.Contains(signUpMethod);
    }
}
