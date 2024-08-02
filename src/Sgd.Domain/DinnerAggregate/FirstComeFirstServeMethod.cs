namespace Sgd.Domain.DinnerAggregate;

public class FirstComeFirstServeMethod : SignUpMethod
{
    public override string Name => "FirstComeFirstServe";

    public override void AddSignUp(Dinner dinner, SignUp signUp)
    {
        if (dinner.SignUps.Count < dinner.Capacity)
            dinner.AddSignUp(signUp);
        else
            dinner.AddToWaitList(signUp);
    }

    public override void ProcessWaitList(Dinner dinner)
    {
        var combinedList = dinner
            .SignUps.Concat(dinner.WaitList)
            .OrderBy(s => s.SignUpDate)
            .ToList();
        dinner.ClearSignUps();
        dinner.ClearWaitList();

        // Re-add hosts to signups first
        foreach (var hostId in dinner.Hosts)
        {
            if (dinner.SignUps.All(s => s.UserId != hostId))
            {
                dinner.AddSignUp(new SignUp(hostId));
            }
        }

        foreach (var signUp in combinedList)
        {
            if (
                dinner.SignUps.All(s => s.UserId != signUp.UserId)
                && dinner.SignUps.Count < dinner.Capacity
            )
            {
                dinner.AddSignUp(signUp);
            }
            else
            {
                dinner.AddToWaitList(signUp);
            }
        }
    }
}
