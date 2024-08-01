using MediatR;
using Microsoft.AspNetCore.Http;
using Sgd.Application.Common.Interfaces;
using Sgd.Domain.Common;
using Sgd.Domain.Common.EventualConsistency;

namespace Sgd.Infrastructure.Middleware;

public class EventualConsistencyMiddleware(RequestDelegate next)
{
    public const string DomainEventsKey = "DomainEventsKey";

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, IUnitOfWork unitOfWork)
    {
        unitOfWork.StartTransaction();
        context.Response.OnCompleted(async () =>
        {
            try
            {
                if (
                    context.Items.TryGetValue(DomainEventsKey, out var value)
                    && value is Queue<IDomainEvent> domainEvents
                )
                {
                    while (domainEvents.TryDequeue(out var nextEvent))
                    {
                        await publisher.Publish(nextEvent);
                    }
                }

                await unitOfWork.CommitTransaction();
            }
            catch (EventualConsistencyException)
            {
                // TODO: Log exception/Handle
                await unitOfWork.RollbackChanges();
            }
            finally { }
        });

        await next(context);
    }
}
