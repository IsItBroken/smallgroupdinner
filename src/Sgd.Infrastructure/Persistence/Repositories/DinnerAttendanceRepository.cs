using Sgd.Application.Common.Interfaces;
using Sgd.Domain.DinnerAggregate;

namespace Sgd.Infrastructure.Persistence.Repositories;

public class DinnerAttendanceRepository(SgdDbContext dbContext, IUnitOfWork unitOfWork)
    : IDinnerAttendanceRepository
{
    public void AddDinnerAttendance(DinnerAttendance dinnerAttendance)
    {
        unitOfWork.AddOperation(
            dinnerAttendance,
            () => dbContext.DinnerAttendances.InsertOneAsync(dinnerAttendance)
        );
    }

    public void UpdateDinnerAttendance(DinnerAttendance dinnerAttendance)
    {
        unitOfWork.AddOperation(
            dinnerAttendance,
            () =>
                dbContext.DinnerAttendances.ReplaceOneAsync(
                    x => x.Id == dinnerAttendance.Id,
                    dinnerAttendance
                )
        );
    }

    public async Task<DinnerAttendance?> GetDinnerAttendanceById(
        ObjectId id,
        CancellationToken cancellationToken
    )
    {
        return await dbContext
            .DinnerAttendances.Find(p => p.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<DinnerAttendance>> GetDinnerAttendancesForUser(
        ObjectId userId,
        CancellationToken cancellationToken
    )
    {
        return await dbContext
            .DinnerAttendances.Find(p => p.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> DoesDinnerAttendanceExist(
        ObjectId userId,
        ObjectId dinnerId,
        CancellationToken cancellationToken
    )
    {
        return await dbContext
            .DinnerAttendances.Find(p => p.UserId == userId && p.DinnerId == dinnerId)
            .AnyAsync(cancellationToken);
    }

    public async Task<List<DinnerAttendance>> GetDinnerAttendancesForDinner(
        ObjectId dinnerId,
        CancellationToken cancellationToken
    )
    {
        return await dbContext
            .DinnerAttendances.Find(p => p.DinnerId == dinnerId)
            .ToListAsync(cancellationToken);
    }
}
