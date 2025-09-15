using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        
        logger.LogInformation("Handling {RequestName}", requestName);
        
        try
        {
            var result = await next();
            logger.LogInformation("Handled {RequestName} successfully", requestName);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling {RequestName}: {Error}", requestName, ex.Message);
            throw;
        }
    }
}
