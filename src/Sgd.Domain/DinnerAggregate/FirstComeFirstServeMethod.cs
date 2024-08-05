namespace Sgd.Domain.DinnerAggregate;

public class FirstComeFirstServeMethod : SignUpMethod
{
    public override string Name => "FirstComeFirstServe";

    public override void AddSignUp(Dinner dinner, SignUp signUp)
    {
        if (dinner.SignUps.Count < dinner.Capacity)
            dinner.AddSignUpFromMethod(signUp);
        else
            dinner.AddToWaitList(signUp);
    }

    public override void ProcessWaitList(Dinner dinner)
    {
        while (dinner.SignUps.Count < dinner.Capacity && dinner.WaitList.Any())
        {
            var signUp = dinner.WaitList[0];
            var result = dinner.MoveFromWaitListToSignUps(signUp);

            if (result.IsError)
            {
                throw new InvalidOperationException(
                    $"Failed to move user from wait list to sign ups, {result.Errors.First().Description}"
                );
            }
        }
    }
}
