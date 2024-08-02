namespace Sgd.Domain.DinnerAggregate;

public abstract class SignUpMethod
{
    public abstract string Name { get; }

    public abstract void AddSignUp(Dinner dinner, SignUp signUp);
    public abstract void ProcessWaitList(Dinner dinner);

    public static SignUpMethod FromString(string signUpMethod)
    {
        return signUpMethod switch
        {
            "FirstComeFirstServe" => new FirstComeFirstServeMethod(),
            "Lottery" => new LotteryMethod(),
            _ => throw new ArgumentException("Invalid sign up method."),
        };
    }
}
