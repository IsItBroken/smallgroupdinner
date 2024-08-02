using Sgd.Domain.Common;
using Sgd.Domain.UserAggregate;

namespace Sgd.Domain.DinnerAggregate;

public class Dinner : AggregateRoot<ObjectId>
{
    public string Name { get; private set; } = null!;

    public DateTime Date { get; private set; }

    public string Description { get; private set; } = null!;

    public string? ImageUrl { get; private set; }

    public int Capacity { get; private set; }

    public SignUpMethod SignUpMethod { get; private set; }

    public DateTime? RandomSelectionTime { get; private set; }

    private List<SignUp> _signUps = [];
    public IReadOnlyList<SignUp> SignUps => _signUps.AsReadOnly();

    private List<SignUp> _waitList = [];
    public IReadOnlyList<SignUp> WaitList => _waitList.AsReadOnly();

    private List<ObjectId> _hosts = [];
    public IReadOnlyList<ObjectId> Hosts => _hosts.AsReadOnly();

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public bool IsDeleted { get; private set; } = false;

    private Dinner(
        string name,
        DateTime date,
        string description,
        int capacity,
        SignUpMethod signUpMethod,
        string? imageUrl,
        DateTime? randomSelectionTime,
        User creator,
        ObjectId? id = null
    )
        : base(id ?? ObjectId.GenerateNewId())
    {
        Name = name;
        Date = date;
        Description = description;
        Capacity = capacity;
        SignUpMethod = signUpMethod;
        ImageUrl = imageUrl;
        RandomSelectionTime = randomSelectionTime;
        _hosts.Add(creator.Id);
    }

    public static Dinner Create(
        string name,
        DateTime date,
        string description,
        int capacity,
        SignUpMethod signUpMethod,
        string? imageUrl,
        DateTime? randomSelectionTime,
        User creator,
        ObjectId? id = null
    )
    {
        var dinner = new Dinner(
            name,
            date,
            description,
            capacity,
            signUpMethod,
            imageUrl,
            randomSelectionTime,
            creator,
            id
        );

        var creatorSignUp = new SignUp(creator.Id);
        dinner._signUps.Add(creatorSignUp);

        return dinner;
    }

    public ErrorOr<Success> AddSignUp(SignUp signUp)
    {
        SignUpMethod.AddSignUp(this, signUp);
        return Result.Success;
    }

    public ErrorOr<Success> RemoveSignUp(ObjectId userId)
    {
        var signUp = _signUps.FirstOrDefault(s => s.UserId == userId);
        if (signUp == null)
        {
            return DinnerErrors.SignUpNotFound;
        }

        _signUps.Remove(signUp);
        return Result.Success;
    }

    public void ProcessWaitList()
    {
        SignUpMethod.ProcessWaitList(this);
    }

    public ErrorOr<Success> AddToWaitList(SignUp signUp)
    {
        if (_waitList.Any(s => s.UserId == signUp.UserId))
        {
            return DinnerErrors.AlreadyInWaitList;
        }
        _waitList.Add(signUp);
        return Result.Success;
    }

    public ErrorOr<Success> RemoveFromWaitList(ObjectId userId)
    {
        var signUp = _waitList.FirstOrDefault(s => s.UserId == userId);
        if (signUp == null)
        {
            return DinnerErrors.SignUpNotFound;
        }

        _waitList.Remove(signUp);
        return Result.Success;
    }

    public ErrorOr<Success> UpdateSignUpMethod(SignUpMethod signUpMethod)
    {
        if (DateTime.UtcNow > RandomSelectionTime)
        {
            // If it's after the random selection time, just update the method
            SignUpMethod = signUpMethod;
        }
        else
        {
            // If it's before the random selection time, process waitlist using the new method
            SignUpMethod = signUpMethod;
            SignUpMethod.ProcessWaitList(this);
        }

        return Result.Success;
    }

    public ErrorOr<Success> UpdateRandomSelectionTime(DateTime newRandomSelectionTime)
    {
        if (newRandomSelectionTime > Date)
        {
            return DinnerErrors.RandomSelectionTimeMustBeBeforeDinnerDate;
        }

        RandomSelectionTime = newRandomSelectionTime;

        // Process the waitlist immediately if the new selection time is in the past
        if (SignUpMethod is LotteryMethod && DateTime.UtcNow > newRandomSelectionTime)
        {
            SignUpMethod.ProcessWaitList(this);
        }

        return Result.Success;
    }

    public ErrorOr<Success> AddHost(ObjectId hostId)
    {
        if (_hosts.Contains(hostId))
        {
            return DinnerErrors.HostAlreadyAdded;
        }

        _hosts.Add(hostId);
        if (SignUps.All(s => s.UserId != hostId))
        {
            AddSignUp(new SignUp(hostId));
        }
        return Result.Success;
    }

    public ErrorOr<Success> RemoveHost(ObjectId hostId)
    {
        if (!_hosts.Contains(hostId))
        {
            return DinnerErrors.HostNotFound;
        }

        if (_hosts.Count == 1)
        {
            return DinnerErrors.CannotRemoveLastHost;
        }

        _hosts.Remove(hostId);
        return Result.Success;
    }

    internal void ClearSignUps()
    {
        _signUps.Clear();
    }

    internal void ClearWaitList()
    {
        _waitList.Clear();
    }

    private Dinner() { }
}
