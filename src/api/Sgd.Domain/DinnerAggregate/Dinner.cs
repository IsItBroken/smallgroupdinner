using Sgd.Domain.Common;
using Sgd.Domain.DinnerAggregate.Events;
using Sgd.Domain.UserAggregate;

namespace Sgd.Domain.DinnerAggregate;

public class Dinner : AggregateRoot<ObjectId>
{
    public string Name { get; private set; } = null!;

    public DateTime Date { get; private set; }

    public string Description { get; private set; } = null!;

    public ObjectId GroupId { get; private set; }

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

    public bool IsCancelled { get; private set; } = false;

    private Dinner(
        string name,
        DateTime date,
        string description,
        ObjectId groupId,
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
        GroupId = groupId;
        Capacity = capacity;
        SignUpMethod = signUpMethod;
        ImageUrl = imageUrl;
        RandomSelectionTime = randomSelectionTime;
        _hosts.Add(creator.Id);

        _domainEvents.Add(new DinnerCreatedEvent(this));
    }

    public static Dinner Create(
        string name,
        DateTime date,
        string description,
        ObjectId groupId,
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
            groupId,
            capacity,
            signUpMethod,
            imageUrl,
            randomSelectionTime,
            creator,
            id
        );

        var creatorSignUp = new SignUp(creator.Id);
        dinner.AddSignUpFromMethod(creatorSignUp);

        return dinner;
    }

    public ErrorOr<Success> UpdateName(string name)
    {
        Name = name;
        return Result.Success;
    }

    public ErrorOr<Success> UpdateDescription(string description)
    {
        Description = description;
        return Result.Success;
    }

    public ErrorOr<Success> UpdateDate(DateTime date)
    {
        var oldDate = Date;
        Date = date;

        _domainEvents.Add(new DinnerDateChangedEvent(this, oldDate));
        return Result.Success;
    }

    public ErrorOr<Success> UpdateCapacity(int capacity)
    {
        if (capacity < SignUps.Count)
        {
            return DinnerErrors.CapacityCannotBeLessThanCurrentSignUps;
        }

        if (capacity < 0)
        {
            return DinnerErrors.CapacityCannotBeNegative;
        }

        SignUpMethod.ProcessWaitList(this);

        Capacity = capacity;
        return Result.Success;
    }

    public ErrorOr<Success> UpdateImageUrl(string? imageUrl)
    {
        ImageUrl = imageUrl;
        return Result.Success;
    }

    public ErrorOr<Success> AddSignUp(SignUp signUp)
    {
        if (IsCancelled)
        {
            return DinnerErrors.DinnerIsCanceled;
        }

        AddSignUpFromMethod(signUp);
        return Result.Success;
    }

    internal ErrorOr<Success> AddSignUpFromMethod(SignUp signUp)
    {
        if (IsCancelled)
        {
            return DinnerErrors.DinnerIsCanceled;
        }

        if (_signUps.Any(s => s.UserId == signUp.UserId))
        {
            return DinnerErrors.AlreadySignedUp;
        }

        _signUps.Add(signUp);
        _domainEvents.Add(new SignedUpForDinnerEvent(this, signUp));
        return Result.Success;
    }

    public ErrorOr<Success> RemoveSignUp(ObjectId userId)
    {
        if (IsCancelled)
        {
            return DinnerErrors.DinnerIsCanceled;
        }

        if (_hosts.Contains(userId))
        {
            return DinnerErrors.HostCannotBeRemoved;
        }

        var signUp = _signUps.FirstOrDefault(s => s.UserId == userId);
        if (signUp == null)
        {
            return DinnerErrors.SignUpNotFound;
        }

        _signUps.Remove(signUp);
        _domainEvents.Add(new CancelledSignUpEvent(this, signUp));
        return Result.Success;
    }

    public ErrorOr<Success> ProcessWaitList()
    {
        if (IsCancelled)
        {
            return DinnerErrors.DinnerIsCanceled;
        }

        SignUpMethod.ProcessWaitList(this);
        return Result.Success;
    }

    public ErrorOr<Success> AddToWaitList(SignUp signUp)
    {
        if (IsCancelled)
        {
            return DinnerErrors.DinnerIsCanceled;
        }

        if (_waitList.Any(s => s.UserId == signUp.UserId))
        {
            return DinnerErrors.AlreadyInWaitList;
        }

        _waitList.Add(signUp);
        _domainEvents.Add(new AddedToDinnerWaitListEvent(this, signUp));
        return Result.Success;
    }

    public ErrorOr<Success> RemoveFromWaitList(ObjectId userId)
    {
        if (IsCancelled)
        {
            return DinnerErrors.DinnerIsCanceled;
        }

        var signUp = _waitList.FirstOrDefault(s => s.UserId == userId);
        if (signUp == null)
        {
            return DinnerErrors.SignUpNotFound;
        }

        _waitList.Remove(signUp);
        return Result.Success;
    }

    public ErrorOr<Success> MoveFromWaitListToSignUps(SignUp signUp)
    {
        if (IsCancelled)
        {
            return DinnerErrors.DinnerIsCanceled;
        }

        if (!_waitList.Contains(signUp))
        {
            return DinnerErrors.SignUpNotFound;
        }

        if (_signUps.Count >= Capacity)
        {
            return DinnerErrors.DinnerIsFull;
        }

        AddSignUpFromMethod(signUp);
        RemoveFromWaitList(signUp.UserId);
        return Result.Success;
    }

    public ErrorOr<Success> UpdateSignUpMethod(SignUpMethod signUpMethod)
    {
        if (IsCancelled)
        {
            return DinnerErrors.DinnerIsCanceled;
        }

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
        if (IsCancelled)
        {
            return DinnerErrors.DinnerIsCanceled;
        }

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
        if (IsCancelled)
        {
            return DinnerErrors.DinnerIsCanceled;
        }

        if (_hosts.Contains(hostId))
        {
            return DinnerErrors.HostAlreadyAdded;
        }

        _hosts.Add(hostId);
        if (SignUps.All(s => s.UserId != hostId))
        {
            var addSignupRequest = AddSignUp(new SignUp(hostId));
            if (addSignupRequest.IsError)
            {
                return addSignupRequest.Errors;
            }
        }

        return Result.Success;
    }

    public ErrorOr<Success> RemoveHost(ObjectId hostId)
    {
        if (IsCancelled)
        {
            return DinnerErrors.DinnerIsCanceled;
        }

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

    public bool CanUserUpdate(User user)
    {
        return _hosts.Contains(user.Id);
    }

    public ErrorOr<Success> CancelDinner()
    {
        if (IsCancelled)
        {
            return DinnerErrors.DinnerAlreadyCanceled;
        }

        if (DateTime.UtcNow > Date)
        {
            return DinnerErrors.DinnerAlreadyHappened;
        }

        IsCancelled = true;
        _domainEvents.Add(new DinnerCanceledEvent(this));
        return Result.Success;
    }

    private Dinner() { }
}
