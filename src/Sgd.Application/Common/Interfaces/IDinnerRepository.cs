using Sgd.Domain.DinnerAggregate;

namespace Sgd.Application.Common.Interfaces;

public interface IDinnerRepository
{
    void AddDinner(Dinner dinner);
    void UpdateDinner(Dinner dinner);
    Task<Dinner?> GetDinnerById(ObjectId id, CancellationToken cancellationToken);
    Task<List<Dinner>> SearchDinners(string? name, CancellationToken cancellationToken);
}
