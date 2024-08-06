using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Common.Interfaces;

public interface IDinnerRepository
{
    void AddDinner(Dinner dinner);
    void UpdateDinner(Dinner dinner);
    Task<Dinner?> GetDinnerById(ObjectId id, CancellationToken cancellationToken);
    Task<List<Dinner>> SearchDinners(
        List<ObjectId> groupIds,
        string? name,
        CancellationToken cancellationToken
    );
    Task<List<Dinner>> GetDinnersByIds(List<ObjectId> ids, CancellationToken cancellationToken);
}
