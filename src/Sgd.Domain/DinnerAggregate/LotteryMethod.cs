namespace Sgd.Domain.DinnerAggregate;

public class LotteryMethod : SignUpMethod
{
    public override string Name => "Lottery";

    public override void AddSignUp(Dinner dinner, SignUp signUp)
    {
        if (DateTime.UtcNow < dinner.RandomSelectionTime)
        {
            dinner.AddToWaitList(signUp);
        }
        else
        {
            // Switch to first come first served after selection time has passed
            var firstComeFirstServe = new FirstComeFirstServeMethod();
            firstComeFirstServe.AddSignUp(dinner, signUp);
        }
    }

    public override void ProcessWaitList(Dinner dinner)
    {
        if (DateTime.UtcNow < dinner.RandomSelectionTime)
        {
            throw new InvalidOperationException("Random selection time has not yet passed.");
        }

        var random = new Random();

        // Ensure all hosts are signed up first
        foreach (var hostId in dinner.Hosts)
        {
            if (dinner.SignUps.All(s => s.UserId != hostId))
            {
                dinner.AddSignUp(new SignUp(hostId));
            }
        }

        var availableSlots = dinner.Capacity - dinner.SignUps.Count;
        var participants = dinner
            .WaitList.OrderBy(_ => random.Next())
            .Take(availableSlots)
            .ToList();

        participants.ForEach(p => dinner.AddSignUp(p));
        dinner.WaitList.Except(participants).ToList().ForEach(p => dinner.AddToWaitList(p));
        dinner.UpdateSignUpMethod(new FirstComeFirstServeMethod());
    }
}
