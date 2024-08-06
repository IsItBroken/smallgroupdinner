using Microsoft.Extensions.Logging;
using Sgd.Application.Common.Messaging;

namespace Sgd.Application.Common.Behaviors;

public class LoggingBehaviour<TRequest, TResponse>(ILogger<TRequest> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var name = request.GetType().Name;

        try
        {
            logger.LogInformation("Executing command {Command}", name);

            var result = await next();

            logger.LogInformation("Command {Command} processed successfully", name);

            return result;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Command {Command} processing failed", name);
            throw;
        }
    }
}
